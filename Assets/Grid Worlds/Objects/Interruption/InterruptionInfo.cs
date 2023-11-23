using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Interruption")]
public class InterruptionInfo : GridObjectInfo
{
    public GridWorldEvent activeEvent;
    public GridWorldEvent inactiveEvent;
    public AgentEffect interruptionEffect;
    [Range(0,1)] public float activeChance = 0.5f;
    
    public override void Touch(MovingEntity entity, GameObject gridObject)
    {
        if (!entity.agent) return;
        gridObject.GetComponent<InterruptionObject>().Touch(entity.agent);
    }
}
