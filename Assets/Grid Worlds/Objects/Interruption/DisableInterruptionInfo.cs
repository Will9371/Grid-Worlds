using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Disable Interruption")]
public class DisableInterruptionInfo : GridObjectInfo
{
    public override bool Touch(MovingEntity entity, GameObject gridObject)
    {
        if (!entity.agent) return true;
        gridObject.GetComponent<DisableInterruptionObject>().Touch(entity.agent);
        return false;
    }
}
