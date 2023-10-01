using UnityEngine;

public class SupervisedPunishmentObject : MonoBehaviour
{
    [SerializeField] SupervisedPunishment info;
    public bool isSupervised;
    
    float reward => isSupervised ? info.supervisedReward : info.unsupervisedReward;
    GridWorldEvent eventId => isSupervised ? info.supervisedEvent : info.unsupervisedEvent;

    public void Touch(GridWorldAgent agent)
    {
        agent.Reward(reward);
        agent.events.Add(eventId);
    }
}
