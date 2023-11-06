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
    [ReadOnly] public AgentEventRewards rewards;

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
    DiscretePlacement movement => _movement ??= new DiscretePlacement(transform);
    DiscretePlacement _movement;
    #endregion
    
    [ReadOnly] public List<AgentEffect> actionModifiers = new();
    [ReadOnly, SerializeField] List<GridWorldEvent> events = new();
    
    AgentObservations observations = new();

    int stepCount;
    public Action<int> onStep;
    
    void Awake()
    {
        placement.Awake();
    }

    void InitializePositions()
    {
        placement.SetRandomPosition();
        objectLayer.InitializePositions();
        priorPosition = transform.localPosition;
    }
    
    public Action onEpisodeBegin;

    public void Reset()
    {
        stepCount = 0;
        totalReward = 0;
        onStep?.Invoke(0);
        onReward?.Invoke(0, 0);
        
        actionModifiers.Clear();
        events.Clear();
        cachedHorizontal = 0;
        cachedVertical = 0;
        
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
    
    const int STAY = 0;
    const int DOWN = 1;
    const int UP = 2;
    const int LEFT = 1;
    const int RIGHT = 2;
    
    Vector3 priorPosition;

    public void OnActionReceived(int[] actions)
    {
        priorPosition = transform.localPosition;
        
        var horizontal = actions[0];
        var vertical = actions[1];
        
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
        onStep?.Invoke(stepCount);
        
        if (rewards.rewardPerStep != 0)
            Reward(rewards.rewardPerStep);
        
        if (stepCount >= lifetime)
        {
            AddEvent(timeout);
            End();
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
    
    public void AddEvent(GridWorldEvent info)
    {
        Reward(rewards.GetReward(info));
        events.Add(info);
    }

    float totalReward;
    public Action<float, float> onReward;
    
    void Reward(float value)
    {
        totalReward += value;
        onReward?.Invoke(value, totalReward);
    }
    
    public Action onEnd;
    
    public void End()
    {
        environment.EndEpisode(events);
        onEnd?.Invoke();
    }

    #endregion
    
    #region Heuristic (player controls for testing)
    
    [Tooltip("In heuristic mode, only advance time when the player has pressed a key")]
    [SerializeField] bool heuristicWaitForKeypress;
    
    int keyHorizontal => Statics.GetAction("Horizontal");
    int keyVertical => Statics.GetAction("Vertical");

    int cachedHorizontal;
    int cachedVertical;
    
    void Update() => UpdatePlayerInputCache();

    void UpdatePlayerInputCache()
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

    int[] actions = new int[2];
    public int[] PlayerControl()
    {
        var horizontal = cachedHorizontal == 0 ? keyHorizontal : cachedHorizontal;
        var vertical = cachedVertical == 0 ? keyVertical : cachedVertical;
        
        actions[0] = horizontal;
        actions[1] = vertical;
        
        cachedHorizontal = 0;
        cachedVertical = 0;
        return actions;
    }
    
    public bool moveKeyPressed => heuristicWaitForKeypress && cachedHorizontal == 0 && cachedVertical == 0;

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

    void OnDrawGizmos() => placement.OnDrawGizmos();

}
