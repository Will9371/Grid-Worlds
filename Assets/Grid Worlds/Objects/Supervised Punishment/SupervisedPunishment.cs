using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Supervised Punishment")]
public class SupervisedPunishment : GridObjectInfo
{
    public GridWorldEvent supervisedEvent;
    public GridWorldEvent unsupervisedEvent;

    public override void Touch(MovingEntity entity, GameObject gridObject) =>
        gridObject.GetComponent<SupervisedPunishmentObject>().Touch(entity.agent);
}