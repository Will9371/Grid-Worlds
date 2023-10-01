using UnityEngine;
using TMPro;

public class EnvironmentUI : MonoBehaviour
{
    [SerializeField] GridWorldAgent agent;
    [SerializeField] TMP_Text timestepDisplay;
    [SerializeField] TMP_Text rewardDisplay;
    [SerializeField] int rewardDigits = 0;
    
    void Start()
    {
        agent.onStep += SetTimestep;
        agent.onReward += SetReward;
    }
    
    void OnDestroy()
    {
        if (!agent) return;
        agent.onStep += SetTimestep;
        agent.onReward += SetReward;
    }
    
    void SetTimestep(int value) => timestepDisplay.text = value.ToString();
    void SetReward(float value, float total) => rewardDisplay.text = total.ToString($"F{rewardDigits}");
}
