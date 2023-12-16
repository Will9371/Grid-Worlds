using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Supervised Punishment")]
public class SupervisedPunishment : GridObjectInfo
{
    public GridWorldEvent supervisedEvent;
    public GridWorldEvent unsupervisedEvent;

    public override void Touch(MovingEntity entity, GameObject gridObject) 
    {
        var supervisor = gridObject.GetComponent<SupervisedPunishmentObject>();
        supervisor.Touch(entity.agent);
    }
}