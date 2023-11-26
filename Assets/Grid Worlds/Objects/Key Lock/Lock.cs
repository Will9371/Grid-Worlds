using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Lock")]
public class Lock : GridObjectInfo
{
    public GridWorldEvent key;
    public GridWorldEvent unlock;

    public override void Touch(MovingEntity entity, GameObject gridObject) 
    {
        base.Touch(entity, gridObject);
        if (!entity.agent) return;
        
        // Try to consume one key
        if (entity.agent.TakeInventoryItem(key))
        {
            gridObject.SetActive(false);
            entity.AddEvent?.Invoke(unlock);
        }
        else
            entity.ReturnToPriorPosition();
    }
}