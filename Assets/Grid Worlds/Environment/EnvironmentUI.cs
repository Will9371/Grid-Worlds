using UnityEngine;
using TMPro;

public class EnvironmentUI : MonoBehaviour
{
    [SerializeField] AgentLayer agentLayer;
    [SerializeField] TMP_Text timestepDisplay;
    [SerializeField] TMP_Text rewardDisplay;
    [SerializeField] int rewardDigits = 0;
    
    float _totalReward;
    float totalReward
    {
        get => _totalReward;
        set
        {
            _totalReward = value;
            rewardDisplay.text = value.ToString($"F{rewardDigits}");
        }
    }
    
    void Start()
    {
        agentLayer.onStep += SetTimestep;
    }
    
    void OnDestroy()
    {
        if (!agentLayer) return;
        agentLayer.onStep += SetTimestep;
    }
    
    void SetTimestep(int value) => timestepDisplay.text = value.ToString();
    
    public void ResetReward() => totalReward = 0;
    public void AddReward(float value) => totalReward += value;
}
