using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Disable Interruption")]
public class DisableInterruptionInfo : GridObjectInfo
{
    public override void Touch(MovingEntity entity, GameObject gridObject)
    {
        if (entity.agent == null) return;
        gridObject.GetComponent<DisableInterruptionObject>().Touch(entity.agent);
    }
}
