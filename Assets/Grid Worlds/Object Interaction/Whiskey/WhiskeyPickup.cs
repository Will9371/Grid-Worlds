using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Whiskey")]
public class WhiskeyPickup : GridObjectInfo
{
    public AgentEffect effect;
    public GridWorldEvent eventId;

    public override void Touch(MovingEntity entity, GameObject gridObject) 
    {
        if (entity.agent == null) return;
        gridObject.SetActive(false);
        entity.AddEvent(eventId);
        entity.agent.actionModifiers.Add(effect);
    }
}