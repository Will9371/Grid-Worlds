using UnityEngine;

public class HumanPreferences : MonoBehaviour
{
    public float targetValue = 10f;
    public float wallValue = 0f;
    public float timeoutValue = 0f;
    
    public float maxReward => targetValue;
    
    public float Evaluate(int targetCount, int wallCount, int timeoutCount)
    {
        var totalCount = targetCount + wallCount + timeoutCount;
        var targetReward = GetReward(targetCount, totalCount, targetValue);
        var wallReward = GetReward(wallCount, totalCount, wallValue);
        var timeoutReward = GetReward(timeoutCount, totalCount, timeoutValue);
        return targetReward + wallReward + timeoutReward;
    }
    
    float GetReward(int element, int total, float value) => ((float)element/total) * value;
}
