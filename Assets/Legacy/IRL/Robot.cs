using System;
using UnityEngine;

// Rename: result tracker (consider moving to IRL agent)
// DEPRECATE
public class Robot : MonoBehaviour
{
    public MoveToGoalAgent agent;

    public int sampleSize = 100;
    public float[] rewards;
    
    public int targetTouchCount;
    public int wallTouchCount;
    public int timeoutCount;
    
    int sampleCount => targetTouchCount + wallTouchCount + timeoutCount;
    
    public void SetResult(MoveToTargetResult result)
    {
        if (sampleCount + 1 >= sampleSize) 
        {
            Broadcast();
            ClearHistory();
        }
        
        switch (result)
        {
            case MoveToTargetResult.Target:
                rewards[sampleCount] = agent.targetReward;
                targetTouchCount++;
                break;
            case MoveToTargetResult.Wall:
                rewards[sampleCount] = agent.wallReward;
                wallTouchCount++;
                break;
            case MoveToTargetResult.Timeout:
                rewards[sampleCount] = agent.timeoutReward;
                timeoutCount++;
                break;
        }
    }

    void ClearHistory()
    {
        targetTouchCount = 0;
        wallTouchCount = 0;
        timeoutCount = 0;
        
        for (int i = 0; i < rewards.Length; i++)
            rewards[i] = 0;        
    }
    
    [SerializeField] int thresholdMultiplier = 3;
    
    void Broadcast()
    {
        var goodTouchCount = agent.targetReward >= agent.wallReward ? targetTouchCount : wallTouchCount;
        var badTouchCount = agent.targetReward < agent.wallReward ? targetTouchCount : wallTouchCount;
        var readyToBroadcast = goodTouchCount >= badTouchCount * thresholdMultiplier;
        if (readyToBroadcast) onBroadcast?.Invoke(targetTouchCount, wallTouchCount);
        //Debug.Log($"targets = {targetTouchCount}, walls = {wallTouchCount}, " +
        //          $"good = {goodTouchCount}, bad = {badTouchCount}, success = {readyToBroadcast}");
    }
    
    public Action<int, int> onBroadcast;
}

/*
[SerializeField] [Range(0f, 1f)] 
float rewardThreshold = 0.5f;

public float average;
void SetAverage()
{
    float sum = 0f;
    foreach (var item in rewards)
        sum += item;
        
    average = sum/rewards.Length;
    
    //Debug.Log($"Robot average: {average.ToString("F3")}");
    if (average >= rewardThreshold)
        onCalculateAverage?.Invoke(goalTouchCount, wallTouchCount); 
}

public Action<int, int> onCalculateAverage;
*/
