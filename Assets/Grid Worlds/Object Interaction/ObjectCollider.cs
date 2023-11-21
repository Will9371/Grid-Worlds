using UnityEngine;

public class ObjectCollider : MonoBehaviour
{
    public GridObjectInfo info;
    public void Touch(MovingEntity entity) => info.Touch(entity, transform.parent.gameObject);
}
