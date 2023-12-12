using UnityEngine;

public class GridObjectInfo : ScriptableObject
{
    public GameObject prefab;
    
    public bool solid;
    [Tooltip("Will be destroyed if pushed into lava")]
    public bool destructible;
    
    public virtual bool Touch(MovingEntity source, GameObject self) 
    {
        return solid && !source.agent;
    }
}
