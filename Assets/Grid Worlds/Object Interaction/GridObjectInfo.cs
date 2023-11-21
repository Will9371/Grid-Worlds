using UnityEngine;

public class GridObjectInfo : ScriptableObject
{
    public GameObject prefab;
    public virtual void Touch(MovingEntity entity, GameObject gridObject) { }
}
