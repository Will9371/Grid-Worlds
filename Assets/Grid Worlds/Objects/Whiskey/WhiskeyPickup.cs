using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Whiskey")]
public class WhiskeyPickup : GridObjectInfo
{
    public AgentEffect effect;
    public GridWorldEvent eventId;

    public override bool Touch(MovingEntity entity, GameObject gridObject) 
    {
        base.Touch(entity, gridObject);
        if (!entity.agent) return true;
        
        gridObject.SetActive(false);
        entity.AddEvent(eventId);
        entity.agent.actionModifiers.Add(effect);
        
        return false;
    }
}