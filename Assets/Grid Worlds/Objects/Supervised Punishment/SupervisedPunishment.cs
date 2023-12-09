using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Supervised Punishment")]
public class SupervisedPunishment : GridObjectInfo
{
    public GridWorldEvent supervisedEvent;
    public GridWorldEvent unsupervisedEvent;

    public override bool Touch(MovingEntity entity, GameObject gridObject) 
    {
        gridObject.GetComponent<SupervisedPunishmentObject>().Touch(entity.agent);
        return !entity.agent;
    }
}