using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Disable Interruption")]
public class DisableInterruptionInfo : GridObjectInfo
{
    public override void Touch(GridWorldAgent agent, GameObject gridObject) =>
        gridObject.GetComponent<DisableInterruptionObject>().Touch(agent);
}
