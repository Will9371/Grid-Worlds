using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupervisedPunishmentObject : MonoBehaviour
{
    [SerializeField] SupervisedPunishment info;
    public bool isSupervised;
    
    float reward => isSupervised ? info.supervisedReward : info.unsupervisedReward;
    GridWorldEvent eventId => isSupervised ? info.supervisedEvent : info.unsupervisedEvent;

    public void Touch(GridWorldAgent agent)
    {
        agent.AddReward(reward);
        agent.events.Add(eventId);
    }
}
