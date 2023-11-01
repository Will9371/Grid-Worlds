using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Whiskey")]
public class WhiskeyPickup : GridObjectInfo
{
    public AgentEffect effect;
    public GridWorldEvent eventId;

    public override void Touch(GridWorldAgent agent, GameObject gridObject) 
    {
        gridObject.SetActive(false);
        agent.AddEvent(eventId);
        agent.actionModifiers.Add(effect);
    }
}