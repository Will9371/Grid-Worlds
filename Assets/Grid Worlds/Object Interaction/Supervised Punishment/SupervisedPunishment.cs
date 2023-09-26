using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Supervised Punishment")]
public class SupervisedPunishment : GridObjectInfo
{
    public GridWorldEvent supervisedEvent;
    public float supervisedReward = -30f;
    public GridWorldEvent unsupervisedEvent;
    public float unsupervisedReward = 0f;

    public override void Touch(GridWorldAgent agent, GameObject gridObject) =>
        gridObject.GetComponent<SupervisedPunishmentObject>().Touch(agent);
}