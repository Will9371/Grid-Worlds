using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class IRLAgent : Agent
{
    [Header("References")]
    [SerializeField] HumanPreferences human;
    [SerializeField] MoveToGoalAgent agent;
    
    [Header("Parameters")]
    [SerializeField] int sampleSize = 100;

    public override void OnEpisodeBegin() 
    {
        RequestDecision();
    }

    // Some observations required for system to run
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agent.targetReward);
        sensor.AddObservation(agent.wallReward);
        sensor.AddObservation(agent.timeoutReward);
    }
    
    /// Not implemented, included to prevent spam warning messages
    public override void Heuristic(in ActionBuffers actionsOut) { }
    
    public override void OnActionReceived(ActionBuffers actions)
    {
        //target.reward = RemapReward(actions.ContinuousActions[0]);
        //wall.reward = RemapReward(actions.ContinuousActions[1]);
        //timeout.reward = RemapReward(actions.ContinuousActions[2]);
        target.reward = actions.ContinuousActions[0];
        wall.reward = actions.ContinuousActions[1];
        timeout.reward = actions.ContinuousActions[2];
        
        //CalibrateRewards();
        
        agent.targetReward = target.reward;
        agent.wallReward = wall.reward;
        agent.timeoutReward = timeout.reward;
    }
    
    #region Reward Calibration

    float RemapReward(float value) => (value + 1) * human.maxReward;
    
    /// Ensure rewards range from +1 to +10
    /// Assumes rewards are positive before reaching this function
    void CalibrateRewards()
    {
        int maxIterations = 10;
        int iterations = 0;
        while (target.reward < 1f && wall.reward < 1f && timeout.reward < 1f && iterations < maxIterations) 
        {
            target.reward *= 10f;
            wall.reward *= 10f;
            timeout.reward *= 10;
            iterations++;
        }
    }
    
    #endregion
    
    public void SetResult(MoveToTargetResult result)
    {
        if (sampleCount + 1 >= sampleSize) 
        {
            EvaluateSample();
            ClearHistory();
            EndEpisode();
        }
        AddSample(result);
    }
    
    float AddSample(MoveToTargetResult result)
    {
        switch (result)
        {
            case MoveToTargetResult.Target: return target.AddSample();
            case MoveToTargetResult.Wall: return wall.AddSample();
            case MoveToTargetResult.Timeout: return timeout.AddSample();
            default: return 0f;
        }
    }

    void ClearHistory()
    {
        target.ClearSamples();
        wall.ClearSamples();
        timeout.ClearSamples();
        agent.episodeCount = 0;
    }
    
    void EvaluateSample()
    {
        var reward = human.Evaluate(target.sampleCount, wall.sampleCount, timeout.sampleCount);
        AddReward(reward);
        
        history.Add(new Episode(reward, target, wall, timeout));
        RequestDecision();
    }

   int sampleCount => target.sampleCount + wall.sampleCount + timeout.sampleCount;
   
   #region Debug
   
   public List<Episode> history;

    [Serializable]
    public struct Episode
    {
        public float irlReward;
        public int targets;
        public float targetReward;
        public int walls;
        public float wallReward;
        public int timeouts;
        public float timeoutReward;
        
        public Episode(float irlReward, Binding target, Binding wall, Binding timeout)
        {
            this.irlReward = irlReward;
            targets = target.sampleCount;
            targetReward = target.reward;
            walls = wall.sampleCount;
            wallReward = wall.reward;
            timeouts = timeout.sampleCount;
            timeoutReward = timeout.reward;
        }
    }
    
    #endregion
    
    #region Binding

    [SerializeField] Binding target;
    [SerializeField] Binding wall;
    [SerializeField] Binding timeout;
    
    [Serializable]
    public class Binding
    {
        [Tooltip("Serialized for debugging")]
        public MoveToTargetResult result;
    
        [Tooltip("Number of times this outcome occured in the last sample of training runs")]
        public int sampleCount;
        
        [Tooltip("Inferred reward value assigned by IRL process")]
        public float reward;
        
        public float AddSample()
        {
            sampleCount++;
            return reward;
        }
        
        public void ClearSamples() => sampleCount = 0;
    }
    
    #endregion
}

/*
//bindingsByReward.Add(target);
//bindingsByReward.Add(wall);
//bindingsByReward.Add(timeout);

//for (int i = 0; i < rewards.Length; i++)
//    rewards[i] = 0;  

//[Header("Serialized for debug")]
//[SerializeField] float[] rewards;
//[SerializeField] List<Binding> bindingsByReward = new();

bindingsByReward = bindingsByReward.OrderByDescending(b => b.reward).ToList();

var goodCount = bindingsByReward[0].sampleCount;
var badCount = 0;
for (int i = 1; i < bindingsByReward.Count; i++)
    badCount += bindingsByReward[i].sampleCount;

var agentTrained = goodCount >= badCount * thresholdMultiplier;
if (agentTrained) 
{
    AddReward(human.Evaluate(target.sampleCount, wall.sampleCount, timeout.sampleCount));
    RequestDecision();
}
*/