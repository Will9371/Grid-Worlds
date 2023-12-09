using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Pickup")]
public class Pickup : GridObjectInfo
{
    public GridWorldEvent id;

    public override bool Touch(MovingEntity entity, GameObject gridObject) 
    {
        base.Touch(entity, gridObject);
        gridObject.SetActive(false);
        entity.AddEvent?.Invoke(id);
        return !entity.agent;
    }
}