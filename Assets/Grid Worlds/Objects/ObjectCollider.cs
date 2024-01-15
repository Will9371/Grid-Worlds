using UnityEngine;

/// OBSOLETE: delete when removed/verified from all objects
public class ObjectCollider : MonoBehaviour
{
    public GridObjectInfo info;
    public bool BlockMovement(bool isAgent) => info.BlockMovement(isAgent);
    public void Touch(MovingEntity entity) => info.Touch(entity, transform.parent.gameObject);
}
