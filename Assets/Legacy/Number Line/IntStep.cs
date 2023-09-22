using System;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class IntStep : Agent
{
    public Binding[] bindings;
    
    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (var binding in bindings)
            sensor.AddObservation(binding.value);
    }
    
    public override void Heuristic(in ActionBuffers actionsOut) { }

    public override void OnActionReceived(ActionBuffers actions)
    {
        foreach (var binding in bindings)
        {
            // Version 1
            /*binding.direction = actions.DiscreteActions[0];
            switch(binding.direction)
            {
                case 0: break;
                case 1: binding.value++; break;
                case 2: binding.value--; break;
            }*/
            
            // Version 2
            //binding.value = actions.DiscreteActions[0];
            
            // Version 3
            binding.value += actions.ContinuousActions[0];
        }
            
        Evaluate();
    }
    
    public float reward;
    void Evaluate()
    {
        reward = 0f;
        foreach (var binding in bindings)
            reward += binding.GetReward();

        AddReward(reward);
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
        //public int direction;

        public float GetReward()
        {
            reward = (-Mathf.Abs(value - target) + offset) * multiplier;
            return reward;
        }
    }
}

    
// Works OK
/*
public Binding testBinding;
public bool up, down, stay;

void OnValidate()
{
    if (stay) testBinding.direction = 0;
    else if (up) 
    {
        testBinding.direction = 1;
        testBinding.value++;
    }
    else if (down)
    {
        testBinding.direction = 2;
        testBinding.value--;
    }
    up = down = stay = false;
    
    testBinding.GetReward();
}*/