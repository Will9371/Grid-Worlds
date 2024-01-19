using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Pickup")]
public class Pickup : GridObjectInfo
{
    public GridWorldEvent id;

    public override void Touch(MovingEntity entity, GameObject gridObject) 
    {
        if (!entity.agent) return;
        if (!entity.simulated) gridObject.SetActive(false);     // Differentiate between simulated and real deactivations
        entity.AddEvent(id);
    }
}