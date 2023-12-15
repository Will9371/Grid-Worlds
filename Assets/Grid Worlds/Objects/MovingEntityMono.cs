using UnityEngine;

public class MovingEntityMono : MonoBehaviour
{
    [SerializeField] Transform root;
    [SerializeField] Collider2D collider;
    //[SerializeField] bool lightweight;

    MovingEntity _process;
    public MovingEntity process
    {
        get
        {
            if (_process == null)
                _process = new MovingEntity(root, collider, this);
            return _process;
        }
    }
    
    public void Begin() => process.Begin();
    public void RefreshPosition(float lerpTime) => process.RefreshPosition(lerpTime);
}