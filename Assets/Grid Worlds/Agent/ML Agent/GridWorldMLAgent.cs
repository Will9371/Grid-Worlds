using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

public class GridWorldMLAgent : Agent
{
    [Header("References")]
    [SerializeField] GridWorldAgent gridWorldAgent;
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

    AgentObservations observations = new();

    float stepDelay => 1f/speed;
    
    void Start()
    {
        gridWorldAgent.onEvent += OnEvent;
        gridWorldAgent.onEnd += End;
    }
    
    void OnDestroy()
    {
        if (gridWorldAgent) 
        {
            gridWorldAgent.onEvent -= OnEvent;
            gridWorldAgent.onEnd -= End;
        }
    }
    
    void OnEvent(GridWorldEvent value)
    {
        Reward(rewards.GetReward(value));
    }
    
    void Reward(float current)
    {
        AddReward(current);
        ui.AddReward(current);
    }

    public override void OnEpisodeBegin() => BeginEpisode();

    void BeginEpisode()
    {
        gridWorldAgent.Reset();
        ui.ResetReward();
        StartCoroutine(Process());        
    }
    
    IEnumerator Process()
    {
        yield return new WaitForSeconds(stepDelay * startDelay);
        var delay = new WaitForSeconds(stepDelay);
        
        while (true)
        {
            if (behavior.BehaviorType == BehaviorType.HeuristicOnly && gridWorldAgent.moveKeyPressed)
            {
                yield return delay;
                continue;
            }
        
            RequestDecision();
            yield return delay;
        }
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        observations = gridWorldAgent.CollectObservations();
                
        foreach (var observation in observations.values)
            sensor.AddObservation(observation.value);
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
        {
            Debug.LogError($"No GridWorldAgent referenced from {gameObject.name}", gameObject);
            return;
        }
    
        behavior.BrainParameters.VectorObservationSize = gridWorldAgent.GetObservationCount();
        gridWorldAgent.onSetScenarioName = SetBehaviorName;
    }
    
    void SetBehaviorName(string value) => behavior.BehaviorName = value;
}