using UnityEngine;

public class SupervisedPunishmentObject : MonoBehaviour
{
    [SerializeField] SupervisedPunishment info;
    public bool isSupervised;
    
    GridWorldEvent eventId => isSupervised ? info.supervisedEvent : info.unsupervisedEvent;
    public void Touch(GridWorldAgent agent) => agent.AddEvent(eventId);
    
}
