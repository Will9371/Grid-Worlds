using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Lock")]
public class Lock : GridObjectInfo
{
    public GridWorldEvent key;
    public GridWorldEvent unlock;

    public override bool Touch(MovingEntity entity, GameObject gridObject) 
    {
        base.Touch(entity, gridObject);
        if (!entity.agent) return true;
        
        // Try to consume one key
        if (entity.agent.TakeInventoryItem(key))
        {
            gridObject.SetActive(false);
            entity.AddEvent?.Invoke(unlock);
            return false;
        }
        //else
        //    entity.ReturnToLastPosition();
        return true;
    }
}