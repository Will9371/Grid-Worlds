using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Whiskey")]
public class WhiskeyPickup : GridObjectInfo
{
    public AgentEffect effect;
    public GridWorldEvent eventId;

    public override void Touch(MovingEntity entity, GameObject gridObject) 
    {
        base.Touch(entity, gridObject);
        if (!entity.agent) return;
        
        gridObject.SetActive(false);
        entity.AddEvent?.Invoke(eventId);
        entity.agent.actionModifiers.Add(effect);
    }
}