using System;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Random = UnityEngine.Random;

public class SeekNumberAgent : Agent
{
    [SerializeField] float reward = 10f;
    [SerializeField] float timeoutReward = 0f;
    [SerializeField] float outOfRangeReward = 0f;
    [SerializeField] float lifetime = 10f;  
    public Binding[] bindings;
    
    float startTime;
    [SerializeField] int episodeCount;
    
    void Start() 
    {
        episodeCount = 0;
        RequestDecision();
    }
    
    public override void OnEpisodeBegin() 
    {
        startTime = Time.time;
        episodeCount++;
        
        foreach (var binding in bindings)
            binding.Initialize();
    }
    
    void Update()
    {
        if (Time.time - startTime < lifetime)
            return;
        
        result = Alignment.Incapable; 
        AddReward(timeoutReward);
        EndEpisode();
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (var binding in bindings)
        {
            sensor.AddObservation(binding.value);
            sensor.AddObservation(binding.target);
        }
    }
    
    public override void Heuristic(in ActionBuffers actionsOut) { }

    public override void OnActionReceived(ActionBuffers actions)
    {
        for (int i = 0; i < bindings.Length; i++)
            bindings[i].value += actions.ContinuousActions[i];
            
        Evaluate();
    }
    
    #pragma warning disable CS0414
    [SerializeField] Alignment result;
    #pragma warning restore CS0414
    
    void Evaluate()
    {
        var atTarget = true;
        var outOfRange = false;
        foreach (var binding in bindings)
        {
            if (!binding.atTarget)
                atTarget = false;
            if (binding.outOfRange)
                outOfRange = true;
        }

        if (atTarget)
        {
            result = Alignment.Aligned; 
            AddReward(reward);
            EndEpisode();
        }
        else if (outOfRange)
        {
            result = Alignment.Unaligned; 
            AddReward(outOfRangeReward);
            EndEpisode();
        }
    }
    
    [Serializable]
    public class Binding
    {
        [Header("Settings")]
        public float range;
        public float tolerance;
        public float maxError;

        [Header("Debug")]
        public float target;
        public float value;

        public void Initialize()
        {
            target = Random.Range(-range, range);
            value = Random.Range(-range, range);
        }
        
        public float error => Mathf.Abs(value - target);
        public bool atTarget => error <= tolerance;
        public bool outOfRange => error > maxError;
    }
}
