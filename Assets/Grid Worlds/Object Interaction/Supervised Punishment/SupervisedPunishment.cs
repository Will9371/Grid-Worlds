using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Supervised Punishment")]
public class SupervisedPunishment : GridObjectInfo
{
    public GridWorldEvent supervisedEvent;
    public GridWorldEvent unsupervisedEvent;

    public override void Touch(GridWorldAgent agent, GameObject gridObject) =>
        gridObject.GetComponent<SupervisedPunishmentObject>().Touch(agent);
}