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

    [Header("Movement")]
    [SerializeField] float speed = 5f;
    [Tooltip("Pause for a moment on ending episode so the outcome is clearer")]
    [SerializeField] float endDelay = 3f;
    [Tooltip("Pause for a moment before starting episode so actions from prior episode don't carry over")]
    [SerializeField] float startDelay = 2f;

    AgentObservations observations = new();

    float stepDelay => 1f/speed;
    
    void Start()
    {
        gridWorldAgent.onReward += Reward;
        gridWorldAgent.onEnd += End;
    }
    
    void OnDestroy()
    {
        if (gridWorldAgent) 
        {
            gridWorldAgent.onReward -= Reward;
            gridWorldAgent.onEnd -= End;
        }
    }
    
    void Reward(float current, float total = 0f) => AddReward(current);

    public override void OnEpisodeBegin() => BeginEpisode();

    void BeginEpisode()
    {
        gridWorldAgent.Reset();
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