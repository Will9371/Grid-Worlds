using UnityEngine;

public class GridObjectInfo : ScriptableObject
{
    public GameObject prefab;
    
    public bool solid;
    [Tooltip("Will be destroyed if pushed into lava")]
    public bool destructible;
    
    public virtual void Touch(MovingEntity source, GameObject self) 
    {
        if (solid && !source.agent)
            source.ReturnToLastPosition();
    }
}
