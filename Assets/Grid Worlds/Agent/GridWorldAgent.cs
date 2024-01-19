using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAgent
{
    void Inject(GridWorldAgent agent);
    void AddEvent(GridWorldEvent value);
    
    void Begin();
    void Step();
    void SimulatedStep();
    void End();
}

public enum ObservationType { BirdsEye, LineOfSight }

[Serializable]
public class GridWorldAgent : MonoBehaviour
{
    public string id;

    [Tooltip("Future Use")]
    public ObservationType observationType = ObservationType.BirdsEye;
    
    [Header("References")]
    [ReadOnly] public ObjectLayer objectLayer;
    [ReadOnly] public CellLayer cellLayer;
    [SerializeField] Collider2D ownCollider;
    [SerializeField] SpriteRenderer sprite;
    public MovingEntity body;
    
    [SerializeField] Color activeColor;
    [SerializeField] Color simulatedColor;
    
    [NonSerialized] public GridWorldEnvironment environment;

    [Header("Starting Position")]
    [SerializeField] RandomizePositionOnBegin placement;
    
    IAgent implementation;
    
    [ReadOnly] public List<AgentEffect> actionModifiers = new();
    [ReadOnly] public List<GridWorldEvent> events = new();
    [ReadOnly, SerializeField] List<GridWorldEvent> inventory = new();
    
    [NonSerialized] public AgentObservations observations = new();
    
    Action<GridWorldAgent> beginComplete;
    Action<GridWorldAgent> stepComplete;
    Action<GridWorldAgent> endComplete;
    public void BeginComplete() => beginComplete?.Invoke(this);
    public void StepComplete() => stepComplete?.Invoke(this);
    public void EndComplete() => endComplete?.Invoke(this);
    
    bool _simulated;
    public bool simulated
    {
        get => _simulated;
        set
        {
            if (value == _simulated) return;
            _simulated = value;
            sprite.color = value ? simulatedColor : activeColor;
            environment.SetSimulated(value);
            body.OnSetSimulated(value);
        }
    }
    
    public void Initialize(Action<GridWorldAgent> beginComplete, Action<GridWorldAgent> stepComplete, Action<GridWorldAgent> endComplete)
    {
        this.beginComplete = beginComplete;
        this.stepComplete = stepComplete;
        this.endComplete = endComplete;
    
        implementation = GetComponent<IAgent>();
        implementation.Inject(this);
        
        body = new MovingEntity(transform, ownCollider, this, this);
        body.environment = environment;

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
        if (!alive) StepComplete();
        else implementation.Step();
    }
    
    public void SimulatedStep() 
    {
        if (!alive) StepComplete();
        else implementation.SimulatedStep();
    }
    
    public ObservationData RefreshObservations() => observations.GetObservationData(this);
    
    public void AddCellObservations(AgentObservations observations)
    {
        ObserveOwnCell(observations);
        
        switch (observationType)
        {
            case ObservationType.BirdsEye:
                objectLayer.AddObservations(observations);
                cellLayer.AddObservations(observations);
                break;
            default: 
                Debug.LogError($"{observationType} not implemented");
                break;
        }
    }
    
    /// Add observations of own state
    public void ObserveOwnCell(AgentObservations sensor) => sensor.Add(Statics.PositionString(transform), "", $"Agent {id}");
    
    public void OnActionReceived(int[] actions)
    {
        var nextPosition = movement.Move(actions);
        //Debug.Log($"action: {actions[0]}");
        body.moveDirection = nextPosition - body.position;
        body.AddToPathIfOpen(nextPosition, false);
        StepComplete();
    }
    
    // * Generate clone and make it carry out the rest...
    public void OnSimulatedActionReceived(int[] actions)
    {
        var nextPosition = movement.Move(actions);
        //Debug.Log($"simulation: {actions[0]}");
        body.moveDirection = nextPosition - body.position;
        body.AddToPathIfOpen(nextPosition, false);
        StepComplete();
    }
    
    public void SetPositionAtEndOfPath(float lerpTime) 
    {
        body.RequestLeaveCell();
        //Debug.Log(environment.simulated);
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
    public string[] ActionNames() => Statics.ActionIdsToNames(ActionSpace());
    
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
    
    public AgentObservationData GetSelfDescription() => new (id, GetInventoryDescriptions(), GetLastStepDescriptions());
    
    public string[] GetInventoryDescriptions() 
    {
        var result = new string[inventory.Count];
        
        for (int i = 0; i < inventory.Count; i++)
            result[i] = inventory[i].name;
            
        return result;
    }
    
    /// TBD:
    public string[] GetLastStepDescriptions() => new [] { "" };
}
