using UnityEngine;

public class GridObjectInfo : ScriptableObject
{
    public GameObject prefab;
    
    public bool solidInteractable;
    public bool destructible;
    
    public virtual void Touch(MovingEntity entity, GameObject gridObject) 
    {
        if (solidInteractable)
            entity.ReturnToPriorPosition();
    }
}
