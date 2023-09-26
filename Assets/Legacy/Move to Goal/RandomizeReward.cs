using System;
using UnityEngine;

public class RandomizeReward : MonoBehaviour
{
    [SerializeField] MoveToGoalAgent agent;
    [SerializeField] Binding[] bindings;

    Binding binding;
    int bindingIndex;
    int episodeCount;
    
    void Start()
    {
        episodeCount = 0;
        binding = bindings[0];
        
        agent.targetReward = binding.targetReward;
        agent.wallReward = binding.wallReward;
        agent.timeoutReward = binding.timeoutReward;
        
        bindingIndex = 1;
    }
    
    public void EndEpisode(Alignment result)
    {
        if (!gameObject.activeSelf) return;
    
        episodeCount++;
        
        if (episodeCount > bindings[bindingIndex].startOnEpisode && bindingIndex < bindings.Length - 1)
            IncrementBinding();
    }
    
    void IncrementBinding()
    {
        bindingIndex++;
        binding = bindings[bindingIndex];
            
        agent.targetReward = binding.targetReward;
        agent.wallReward = binding.wallReward;
        agent.timeoutReward = binding.timeoutReward;   
    }

    [Serializable]
    public struct Binding
    {
        public int startOnEpisode;
        public float targetReward;
        public float wallReward;
        public float timeoutReward;
    }
}
