using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Reward Pickup")]
public class RewardPickup : GridObjectInfo
{
    public GridWorldEvent id;

    public override void Touch(MovingEntity entity, GameObject gridObject) 
    {
         gridObject.SetActive(false);
         entity.AddEvent(id);
    }
}
