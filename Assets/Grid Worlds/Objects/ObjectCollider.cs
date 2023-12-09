using UnityEngine;

public class ObjectCollider : MonoBehaviour
{
    public GridObjectInfo info;
    public bool Touch(MovingEntity entity) => info.Touch(entity, transform.parent.gameObject);
}
