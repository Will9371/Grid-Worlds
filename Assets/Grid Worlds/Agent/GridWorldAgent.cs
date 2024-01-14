using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAgent
{
    void Inject(GridWorldAgent agent);
    void AddEvent(GridWorldEvent value);
    
    void Begin();
    void Step();
    void End();
}

public enum ObservationType { BirdsEye, LineOfSight }

[Serializable]
public class GridWorldAgent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int lifetime = 30;
    [Tooltip("Lookup for rewards for various events")]

    [Header("Observations")]
    ObservationType observationType;
    //[SerializeField] bool observeSelf = true;
    //[SerializeField] bool observeObjects = true;
    //[SerializeField] bool observeCells = true;

    [Header("Events")]
    public GridWorldEvent timeout;
    
    [Header("References")]
    [ReadOnly] public ObjectLayer objectLayer;
    [ReadOnly] public CellLayer cellLayer;
    [SerializeField] Collider2D ownCollider;
    public MovingEntity body;

    [Header("Starting Position")]
    [SerializeField] RandomizePositionOnBegin placement;
    
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
    
    IAgent implementation;
    #endregion
    
    [ReadOnly] public List<AgentEffect> actionModifiers = new();
    [ReadOnly] public List<GridWorldEvent> events = new();
    [ReadOnly, SerializeField] List<GridWorldEvent> inventory = new();
    
    AgentObservations observations = new();
    
    Action<GridWorldAgent> beginComplete;
    Action<GridWorldAgent> stepComplete;
    Action<GridWorldAgent> endComplete;
    public void BeginComplete() => beginComplete?.Invoke(this);
    public void StepComplete() => stepComplete?.Invoke(this);
    public void EndComplete() => endComplete?.Invoke(this);

    int stepCount;
    public Action<int> onStep;
    
    public void Initialize(Action<GridWorldAgent> beginComplete, Action<GridWorldAgent> stepComplete, Action<GridWorldAgent> endComplete)
    {
        this.beginComplete = beginComplete;
        this.stepComplete = stepComplete;
        this.endComplete = endComplete;
    
        implementation = GetComponent<IAgent>();
        implementation.Inject(this);
        
        body = new MovingEntity(transform, ownCollider, this, this);

        switch (moveType)
        {
            case AgentMovementType.Axis2Direction8 : movement = new AgentMovement2Axis(); break;
            case AgentMovementType.Direction4 : movement = new AgentMovement4Direction(); break;
        }
        movement.Awake(new DiscretePlacement(transform), actionModifiers);
        
        placement.Awake();
    }
    
    public void Begin()
    {
        stepCount = 0;
        onStep?.Invoke(0);
        
        actionModifiers.Clear();
        events.Clear();
        inventory.Clear();
        movement.ClearCache();
        
        placement.SetRandomPosition();
        body.Begin();
        
        alive = true;
        implementation.Begin();
    }

    #region I/O
    
    public void Step() 
    {
        if (!alive) return;
        implementation.Step();
    }
    
    public AgentObservations CollectObservations()
    {
        observations.Clear();
        
        switch (observationType)
        {
            case ObservationType.BirdsEye:
                AddObservations(observations);
                objectLayer.AddObservations(observations);
                cellLayer.AddObservations(observations);
                break;
        }
                
        return observations;
    }
    
    public void AddObservations(AgentObservations sensor) => sensor.Add(Statics.PositionString(transform), "", "Agent");
        //placement.AddObservations(sensor);
    
    public void OnActionReceived(int[] actions)
    {
        var nextPosition = movement.Move(actions);
        body.moveDirection = nextPosition - body.position;
        body.AddToPathIfOpen(nextPosition, false);

        stepCount++;
        onStep?.Invoke(stepCount);
        
        if (stepCount >= lifetime)
        {
            AddEvent(timeout);
            End();
        }
        
        StepComplete();
    }
    
    public void SetPositionAtEndOfPath(float lerpTime) 
    {
        body.RequestLeaveCell();
        body.RefreshPosition(lerpTime);
    }
    
    #endregion
    
    public void AddEvent(GridWorldEvent info)
    {
        events.Add(info);
        
        if (info.inventoryItem)
            inventory.Add(info);
        
        implementation.AddEvent(info);
    }
    
    [ReadOnly] public bool alive;
    
    public void End()
    { 
        alive = false;
        implementation.End();
        //Debug.Log("GridWorldAgent.End()");
    }
    
    public AgentMovementType moveType;
    public IAgentMovement movement;
    
    void Update() => movement.Update();
    public int[] PlayerControl() => movement.PlayerControl();
    public bool MoveKeyPressed() => movement.MoveKeyPressed();
    public int[] ActionSpace() => movement.ActionSpace();
    public void ApplyPlayerControl() => OnActionReceived(PlayerControl());
    
    public int GetObservationCount()
    {
        placement.transform = transform;
        if (transform.parent == null || transform.parent.name == "Prefab Mode in Context") return -1;
        return Statics.spatialDimensions + objectLayer.GetObservationCount() + cellLayer.ObservationCount();
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
}
