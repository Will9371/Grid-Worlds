using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Pickup")]
public class Pickup : GridObjectInfo
{
    public GridWorldEvent id;

    public override void Touch(MovingEntity entity, GameObject gridObject) 
    {
        if (!entity.agent) return;
        
        if (entity.simulated)
            gridObject.GetComponent<GridObject>().SetVisible(false);
        else 
            gridObject.SetActive(false);
        
        entity.AddEvent(id);
    }
}