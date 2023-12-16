using UnityEngine;

public class GridObjectInfo : ScriptableObject
{
    public GameObject prefab;
    
    [Tooltip("[Broken] Purpose is to block object movement but not agent")]
    public bool solid;
    [Tooltip("Will be destroyed if pushed into lava")]
    public bool destructible;
    
    public virtual bool BlockMovement(bool sourceIsAgent) => solid || !sourceIsAgent;
    public virtual void Touch(MovingEntity source, GameObject self) { }
}
