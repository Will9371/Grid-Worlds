using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

/// BROKEN: fix to accomodate generality for Web agents
public class GridWorldMLAgent : Agent, IAgent
{
    [Header("References")]
    GridWorldAgent gridWorldAgent;
    [SerializeField] BehaviorParameters behavior;
    [SerializeField] EnvironmentUI ui;

    [Header("Movement")]
    [SerializeField] float speed = 5f;
    [Tooltip("Pause for a moment on ending episode so the outcome is clearer")]
    [SerializeField] float endDelay = 3f;
    [Tooltip("Pause for a moment before starting episode so actions from prior episode don't carry over")]
    [SerializeField] float startDelay = 2f;
    
    [Header("Parameters")]
    public AgentEventRewards rewards;
    
    float stepDelay => 1f/speed;
    
    public void Inject(GridWorldAgent agent) => gridWorldAgent = agent;
    
    public void AddEvent(GridWorldEvent value) => Reward(rewards.GetReward(value));
    
    void Reward(float current)
    {
        AddReward(current);
        ui.AddReward(current);
    }

    public override void OnEpisodeBegin() => Begin();

    public void Begin()
    {
        //gridWorldAgent.Reset();
        ui.ResetReward();
        StartCoroutine(Process());        
    }
    
    IEnumerator Process()
    {
        yield return new WaitForSeconds(stepDelay * startDelay);
        var delay = new WaitForSeconds(stepDelay);
        
        while (true)
        {
            if (behavior.BehaviorType == BehaviorType.HeuristicOnly && gridWorldAgent.MoveKeyPressed())
            {
                yield return delay;
                continue;
            }
        
            RequestDecision();
            yield return delay;
        }
    }
    
    // TBD: implement centralized timer refactor
    public void Step() { } 
    public void SimulatedStep() { }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        //observations = gridWorldAgent.CollectObservations();
                
        //foreach (var observation in observations.values)
        //    sensor.AddObservation(observation.value);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionData = new int[actions.DiscreteActions.Length];
        
        for (int i = 0; i < actionData.Length; i++)
            actionData[i] = actions.DiscreteActions[i];
            
        gridWorldAgent.OnActionReceived(actionData);
        
        if (rewards.rewardPerStep != 0)
            Reward(rewards.rewardPerStep);
    }
    
    public void End()
    {
        StopAllCoroutines();
        Invoke(nameof(EndEpisode), stepDelay * endDelay);
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var playerInput = gridWorldAgent.PlayerControl();
        ActionSegment<int> actionsIn = actionsOut.DiscreteActions;
        
        for (int i = 0; i < actionsIn.Length; i++)
            actionsIn[i] = playerInput[i];
    }

    void OnValidate()
    {
        if (behavior == null)
        {
            Debug.LogError($"No behavior referenced from {gameObject.name}", gameObject);
            return;
        }
        if (gridWorldAgent == null)
            gridWorldAgent = GetComponent<GridWorldAgent>();
        if (gridWorldAgent == null)
        {
            Debug.LogError($"No GridWorldAgent referenced from {gameObject.name}", gameObject);
            return;
        }
    
        behavior.BrainParameters.VectorObservationSize = gridWorldAgent.GetObservationCount();
        behavior.BrainParameters.ActionSpec = new ActionSpec(0, GetActionSpace(gridWorldAgent.moveType));
        gridWorldAgent.onSetScenarioName = SetBehaviorName;
    }
    
    int[] GetActionSpace(AgentMovementType moveType)
    {
        switch (moveType)
        {
            case AgentMovementType.Axis2Direction8: return new[] { 3, 3 };
            case AgentMovementType.Direction4: return new[] { 5 };
            default: return new[] { 0 };
        }
    }
    
    void SetBehaviorName(string value) => behavior.BehaviorName = value;
}