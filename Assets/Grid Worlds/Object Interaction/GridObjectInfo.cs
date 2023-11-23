using UnityEngine;

public class GridObjectInfo : ScriptableObject
{
    public GameObject prefab;
    
    public bool solid;
    public bool destructible;
    
    public virtual void Touch(MovingEntity source, GameObject self) 
    {
        if (solid && !source.agent)
            source.ReturnToPriorPosition();
    }
}
