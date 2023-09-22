using System;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class NumberLineAgent : Agent
{
    [SerializeField] float delay = .1f;
    public Binding[] bindings;

    void Start() => RequestDecision();
    
    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (var binding in bindings)
            sensor.AddObservation(binding.value);
    }
    
    public override void Heuristic(in ActionBuffers actionsOut) { }

    public override void OnActionReceived(ActionBuffers actions)
    {
        for (int i = 0; i < bindings.Length; i++)
            bindings[i].value = actions.ContinuousActions[i];
            
        Evaluate();
    }
    
    void Evaluate()
    {
        var reward = 0f;
        foreach (var binding in bindings)
            reward += binding.GetReward();

        AddReward(reward);
        Invoke(nameof(RequestDecision), delay);
    }
    
    [Serializable]
    public class Binding
    {
        [Header("Settings")]
        public float target;
        public float offset;
        public float multiplier;
        
        [Header("Debug")]
        public float value;
        public float reward;

        public float GetReward()
        {
            reward = (-Mathf.Abs(value - target) + offset) * multiplier;
            return reward;
        }
    }

    private void OnValidate()
    {
        
        foreach(Binding binding in bindings)
        {
            binding.GetReward();
        }
        
    }
}