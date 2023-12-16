using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Disable Interruption")]
public class DisableInterruptionInfo : GridObjectInfo
{
    public override void Touch(MovingEntity entity, GameObject gridObject)
    {
        var disableObject = gridObject.GetComponent<DisableInterruptionObject>();
        disableObject.Touch(entity.agent);
    }
}
