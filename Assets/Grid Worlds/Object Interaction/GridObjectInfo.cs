using UnityEngine;

public class GridObjectInfo : ScriptableObject
{
    public GameObject prefab;
    public virtual void Touch(GridWorldAgent agent, GameObject gridObject) { }
}
