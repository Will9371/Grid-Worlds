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
    public MovingEntity body;

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
                if (!transform.parent.parent)
                    Debug.LogError("Agent has no environment!");
                    
                _environment = transform.parent.parent.GetComponent<GridWorldEnvironment>();
                
                if (!_environment)
                    Debug.LogError($"No GridWorldEnvironment attached to parent {transform.parent.name} of GridWorldAgent", transform.parent.parent.gameObject);
            }
            
            return _environment;
        }
    }
    GridWorldEnvironment _environment;
    #endregion
    
    [ReadOnly] public List<AgentEffect> actionModifiers = new();
    [ReadOnly, SerializeField] List<GridWorldEvent> events = new();
    [ReadOnly, SerializeField] List<GridWorldEvent> inventory = new();
    
    AgentObservations observations = new();

    int stepCount;
    public Action<int> onStep;
    
    void Awake()
    {
        placement.Awake();
        body = new MovingEntity(transform, ownCollider, this);
        body.AddEvent = AddEvent;
        body.End = End;
        
        switch (moveType)
        {
            case AgentMovementType.Axis2Direction8 : movement = new AgentMovement2Axis(); break;
            case AgentMovementType.Direction4 : movement = new AgentMovement4Direction(); break;
        }
        movement.Awake(new DiscretePlacement(transform), actionModifiers);
        
        alive = true;
    }

    void InitializePositions()
    {
        placement.SetRandomPosition();
        body.SetPriorPosition();
    }
    
    public Action onEpisodeBegin;

    public void Reset()
    {
        stepCount = 0;
        onStep?.Invoke(0);
        
        actionModifiers.Clear();
        events.Clear();
        inventory.Clear();
        movement.ClearCache();
        
        InitializePositions();
        environment.BeginEpisode();
        
        episodeCount++;
        onEpisodeBegin?.Invoke();
        
        alive = true;
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
    
    public void OnActionReceived(int[] actions)
    {
        body.SetPriorPosition();
        movement.Move(actions);

        stepCount++;
        onStep?.Invoke(stepCount);
        
        if (stepCount >= lifetime)
        {
            AddEvent(timeout);
            End();
        }
        
        body.CheckForColliders();
        body.RequestLeaveCell();
    }
    
    #endregion
    
    public Action<GridWorldEvent> onEvent;
    
    public void AddEvent(GridWorldEvent info)
    {
        events.Add(info);
        
        if (info.inventoryItem)
            inventory.Add(info);
        
        onEvent?.Invoke(info);
    }
    
    public Action onEnd;
    [ReadOnly] public bool alive;
    
    public void End()
    {
        alive = false;
        environment.EndEpisode(events);
        onEnd?.Invoke();
    }
    
    public AgentMovementType moveType;
    public IAgentMovement movement;
    
    void Update() => movement.Update();
    public int[] PlayerControl() => movement.PlayerControl();
    public bool MoveKeyPressed() => movement.MoveKeyPressed();
    public int[] ActionSpace() => movement.ActionSpace();
    
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
    
    public bool TakeInventoryItem(GridWorldEvent item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            return true;
        }
        return false;
    }
    
    public Action onClearBeginFlag;
    public Action onClearStepFlag;
    public Action onClearEndFlag;
}
