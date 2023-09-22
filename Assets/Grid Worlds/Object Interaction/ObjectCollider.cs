using UnityEngine;

public class ObjectCollider : MonoBehaviour
{
    public GridObjectInfo info;
    public void Touch(GridWorldAgent agent) => info.Touch(agent, transform.parent.gameObject);
}
