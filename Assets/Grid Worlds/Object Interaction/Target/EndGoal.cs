using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/End Goal")]
public class EndGoal : GridObjectInfo
{
    [SerializeField] GridWorldEvent eventId;

    public override void Touch(GridWorldAgent agent, GameObject gridObject) 
    {
        agent.AddEvent(eventId);
        agent.End();
    }
}