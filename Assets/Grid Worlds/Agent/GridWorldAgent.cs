using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

public class GridWorldAgent : Agent
{
    [Header("Settings")]
    [SerializeField] int lifetime = 30;
    [SerializeField] float speed = 5f;
    [SerializeField] float endDelay = 3f;
    
    [Header("Observations")]
    [SerializeField] bool observeSelf = true;
    [SerializeField] bool observeObjects = true;
    [SerializeField] bool observeCells = true;

    [Header("Timeout")]
    public float timeoutReward = 0f;
    public GridWorldEvent timeout;
    
    [Header("References")]
    [SerializeField] ObjectLayer objectLayer;

    [Header("Starting Position")]
    [SerializeField] RandomizePositionOnBegin placement;

    [Header("Debug")]
    [SerializeField] int episodeCount;
    
    #region Dependencies
    GridWorldEnvironment environment
    {
        get
        {
            if (_environment == null)
            {
                if (transform.parent == null)
                    Debug.LogError("Agent has no environment!");
                    
                _environment = transform.parent.GetComponent<GridWorldEnvironment>();
                
                if (_environment == null)
                    Debug.LogError($"No GridWorldEnvironment attached to parent {transform.parent.name} of GridWorldAgent", transform.parent.gameObject);
            }
            
            return _environment;
        }
    }
    GridWorldEnvironment _environment;
    BehaviorParameters behavior
    {
        get
        {
            if (_behavior == null)
                _behavior = GetComponent<BehaviorParameters>();
            return _behavior;
        }
    }
    BehaviorParameters _behavior;
    DiscretePlacement movement => _movement ??= new DiscretePlacement(transform);
    DiscretePlacement _movement;
    #endregion
    
    //[HideInInspector] 
    public List<AgentEffect> actionModifiers = new();
    //[HideInInspector]
    public List<GridWorldEvent> events = new();

    int stepCount;
    float stepDelay => 1f/speed;
    
    void Awake()
    {
        placement.Awake();
    }

    void InitializePositions()
    {
        placement.SetRandomPosition();
        objectLayer.InitializePositions();
    }
    
    public Action onEpisodeBegin;
    
    public override void OnEpisodeBegin()
    {
        stepCount = 0;
        actionModifiers.Clear();
        events.Clear();
        InitializePositions();
        episodeCount++;
        onEpisodeBegin?.Invoke();
        StartCoroutine(Process());
    }
    
    IEnumerator Process()
    {
        var delay = new WaitForSeconds(stepDelay);
        
        while (true)
        {
            RequestDecision();
            yield return delay;
        }
    }
    
    #region I/O
    
    public override void CollectObservations(VectorSensor sensor)
    {
        if (observeSelf)
            AddObservations(sensor);

        if (observeObjects)
            objectLayer.AddObservations(sensor);

        if (observeCells)
            foreach (var cell in environment.cells)
                cell.AddObservations(sensor);
    }
    
    public void AddObservations(VectorSensor sensor)
    {
        placement.AddObservations(sensor);
    }
    
    const int STAY = 0;
    const int DOWN = 1;
    const int UP = 2;
    const int LEFT = 1;
    const int RIGHT = 2;
    
    Vector3 priorPosition;

    public override void OnActionReceived(ActionBuffers actions)
    {
        priorPosition = transform.localPosition;
        
        var horizontal = actions.DiscreteActions[0];
        var vertical = actions.DiscreteActions[1];
        
        foreach (var modifier in actionModifiers)
            modifier.ModifyActions(ref horizontal, ref vertical);
    
        switch(horizontal)
        {
            case STAY: break;
            case LEFT: movement.MoveLeft(); break;
            case RIGHT: movement.MoveRight(); break;
        }
        switch(vertical)
        {
            case STAY: break;
            case DOWN: movement.MoveDown(); break;
            case UP: movement.MoveUp(); break;
        }

        stepCount++;
        if (stepCount >= lifetime)
        {
            events.Add(timeout);
            End(timeoutReward);
        }
    }
    
    #endregion
    
    #region Contact & Rewards
    
    void OnTriggerEnter2D(Collider2D other)
    {
        var cell = other.GetComponent<GridCell>();
        if (cell != null) cell.Touch(this);

        var gridObject = other.GetComponent<ObjectCollider>();
        if (gridObject != null) gridObject.Touch(this);
    }
    
    public void ReturnToPriorPosition() => transform.localPosition = priorPosition;
    
    public void End(float reward)
    {
        AddReward(reward);
        StopAllCoroutines();
        Invoke(nameof(EndEpisode), stepDelay * endDelay);
        environment.EndEpisode(events);
    }
    
    #endregion
    
    #region Heuristic (player controls for testing)
    
    int keyHorizontal => Statics.GetAction("Horizontal");
    int keyVertical => Statics.GetAction("Vertical");

    int cachedHorizontal;
    int cachedVertical;
    
    void Update()
    {
        if (cachedHorizontal == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                cachedHorizontal = LEFT;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                cachedHorizontal = RIGHT;
        }
        if (cachedVertical == 0)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                cachedVertical = DOWN;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                cachedVertical = UP;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var horizontal = cachedHorizontal == 0 ? keyHorizontal : cachedHorizontal;
        var vertical = cachedVertical == 0 ? keyVertical : cachedVertical;
        
        ActionSegment<int> actionsIn = actionsOut.DiscreteActions;
        actionsIn[0] = horizontal;
        actionsIn[1] = vertical;
        
        cachedHorizontal = 0;
        cachedVertical = 0;
    }
    
    #endregion
    
    const int spatialDimensions = 2;
    int selfObservationCount => observeSelf ? spatialDimensions : 0;
    int objectObservationCount => observeObjects && objectLayer != null ? objectLayer.GetObservationCount() : 0;
    int spaceObservationCount => observeCells && environment != null && environment.cells != null ? (spatialDimensions + 1) * environment.cells.Length : 0;
    
    void OnValidate()
    {
        placement.transform = transform;
        if (transform.parent == null || transform.parent.name == "Prefab Mode in Context") return;
        behavior.BrainParameters.VectorObservationSize = selfObservationCount + objectObservationCount + spaceObservationCount;
    }

    void OnDrawGizmos() => placement.OnDrawGizmos();
}