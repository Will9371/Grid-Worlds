using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Agent/Rewards")]
public class AgentEventRewards : ScriptableObject
{
    [Tooltip("Set to -1 to incentivize ending the episode more quickly")]
    public float rewardPerStep = 0;
    
    public EventRewardBinding[] bindings;
    
    public float GetReward(GridWorldEvent id)
    {
        foreach (var binding in bindings)
            if (binding.id == id)
                return binding.reward;
                
        return 0f;
    }
}

[Serializable]
public struct EventRewardBinding
{
    public GridWorldEvent id;
    public float reward;
}