using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/End Goal")]
public class EndGoal : GridObjectInfo
{
    [SerializeField] float reward;
    [SerializeField] GridWorldEvent eventId;

    public override void Touch(GridWorldAgent agent, GameObject gridObject) 
    {
        agent.events.Add(eventId);
        agent.End(reward);
    }
}