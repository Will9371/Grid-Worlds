using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridWorldAgent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int lifetime = 30;
    [Tooltip("Lookup for rewards for various events")]

    [Header("Observations")]
    [SerializeField] bool observeSelf = true;
    [SerializeField] bool observeObjects = true;
    [SerializeField] bool observeCells = true;

    [Header("Events")]
    public GridWorldEvent timeout;
    
    [Header("References")]
    [ReadOnly] public ObjectLayer objectLayer;
    [SerializeField] Collider2D ownCollider;

    [Header("Starting Position")]
    [SerializeField] RandomizePositionOnBegin placement;

    [Header("Debug")]
    [SerializeField, ReadOnly] int episodeCount;
    
    #region Dependencies
    GridWorldEnvironment environment
    {
        get
        {
            if (!_environment)
            {
                if (!transform.parent)
                    Debug.LogError("Agent has no environment!");
                    
                _environment = transform.parent.GetComponent<GridWorldEnvironment>();
                
                if (!_environment)
                    Debug.LogError($"No GridWorldEnvironment attached to parent {transform.parent.name} of GridWorldAgent", transform.parent.gameObject);
            }
            
            return _environment;
        }
    }
    GridWorldEnvironment _environment;
    #endregion
    
    [ReadOnly] public List<AgentEffect> actionModifiers = new();
    [ReadOnly, SerializeField] List<GridWorldEvent> events = new();
    
    AgentObservations observations = new();

    int stepCount;
    public Action<int> onStep;
    
    void Awake()
    {
        placement.Awake();
        
        switch (moveType)
        {
            case AgentMovementType.Axis2Direction8 : movement = new AgentMovement2Axis(); break;
            case AgentMovementType.Direction4 : movement = new AgentMovement4Direction(); break;
        }
        movement.Awake(new DiscretePlacement(transform), actionModifiers);
    }

    void InitializePositions()
    {
        placement.SetRandomPosition();
        objectLayer.InitializePositions();
        priorPosition = position;
    }
    
    public Action onEpisodeBegin;

    public void Reset()
    {
        stepCount = 0;
        onStep?.Invoke(0);
        
        actionModifiers.Clear();
        events.Clear();
        movement.ClearCache();
        
        InitializePositions();
        
        episodeCount++;
        onEpisodeBegin?.Invoke();
    }

    #region I/O
    
    public AgentObservations CollectObservations()
    {
        observations.Clear();

        if (observeSelf)
            AddObservations(observations);

        if (observeObjects)
            objectLayer.AddObservations(observations);

        if (observeCells)
            foreach (var cell in environment.cellLayer.cells)
                cell.AddObservations(observations);
                
        return observations;
    }
    
    public void AddObservations(AgentObservations sensor) => placement.AddObservations(sensor);
    
    [HideInInspector] public Vector3 priorPosition;
    public Vector3 position => transform.localPosition;
    
    public void OnActionReceived(int[] actions)
    {
        //var result = "";
        //foreach (var item in actions)
        //    result += item.ToString();
        //Debug.Log($"{result}, {Time.time}");  // OK
    
        priorPosition = position;
        movement.Move(actions);

        stepCount++;
        onStep?.Invoke(stepCount);
        
        if (stepCount >= lifetime)
        {
            AddEvent(timeout);
            End();
        }
        
        var colliders = Physics2D.OverlapCircleAll(transform.position, .1f);
        foreach (var collider in colliders)
        {
            if (collider == ownCollider) continue;
            OnTouch2D(collider);
        }
    }
    
    #endregion
    
    #region Contact & Rewards
    
    void OnTouch2D(Collider2D other)
    {
        var cell = other.GetComponent<GridCell>();
        if (cell) cell.Touch(this);

        var gridObject = other.GetComponent<ObjectCollider>();
        if (gridObject) gridObject.Touch(this);
    }
    
    public void ReturnToPriorPosition() => transform.localPosition = priorPosition;
    
    public Action<GridWorldEvent> onEvent;
    
    public void AddEvent(GridWorldEvent info)
    {
        events.Add(info);
        onEvent?.Invoke(info);
    }
    
    public Action onEnd;
    
    public void End()
    {
        environment.EndEpisode(events);
        onEnd?.Invoke();
    }

    #endregion
    
    #region Heuristic (player controls for testing)
    
    public AgentMovementType moveType;
    public IAgentMovement movement;
    
    void Update() => movement.Update();
    public int[] PlayerControl() => movement.PlayerControl();
    public bool MoveKeyPressed() => movement.MoveKeyPressed();

    #endregion
    
    const int spatialDimensions = 2;
    int selfObservationCount => observeSelf ? spatialDimensions : 0;
    int objectObservationCount => observeObjects && objectLayer != null ? objectLayer.GetObservationCount() : 0;
    int spaceObservationCount => observeCells && environment != null && environment.cells != null ? (spatialDimensions + 1) * environment.cells.Length : 0;
    
    public int GetObservationCount()
    {
        placement.transform = transform;
        if (transform.parent == null || transform.parent.name == "Prefab Mode in Context") return -1;
        return selfObservationCount + objectObservationCount + spaceObservationCount;
    }
    
    public Action<string> onSetScenarioName;

    void OnDrawGizmos() => placement.OnDrawGizmos();
}
