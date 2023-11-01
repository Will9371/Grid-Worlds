using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Reward Pickup")]
public class RewardPickup : GridObjectInfo
{
    public GridWorldEvent id;

    public override void Touch(GridWorldAgent agent, GameObject gridObject) 
    {
         gridObject.SetActive(false);
         agent.AddEvent(id);
    }
}
