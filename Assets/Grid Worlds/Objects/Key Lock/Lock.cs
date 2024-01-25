using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Lock")]
public class Lock : GridObjectInfo
{
    public GridWorldEvent key;
    public GridWorldEvent unlock;
    
    public override void Touch(MovingEntity entity, GameObject gridObject) 
    {
        if (!entity.agent) return;
        
        // Condition: attempt to remove a key from inventory
        var consumeKey = entity.agent.TakeInventoryItem(key);
        if (!consumeKey) return;
        
        // Success: lock opens
        Debug.Log(entity.simulated);
        if (entity.simulated)
            gridObject.GetComponent<GridObject>().SetVisible(false);
        else 
            gridObject.SetActive(false);
            
        entity.AddEvent(unlock);
    }
}