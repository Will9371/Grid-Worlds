using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Lock")]
public class Lock : GridObjectInfo
{
    public GridWorldEvent key;
    public GridWorldEvent unlock;
    
    public override void Touch(MovingEntity entity, GameObject gridObject) 
    {
        var consumeKey = entity.agent.TakeInventoryItem(key);
        if (!consumeKey) return;
        
        // Success: lock opens
        gridObject.SetActive(false);
        entity.AddEvent(unlock);
    }
}