using UnityEngine;

public class MovingEntityMono : MonoBehaviour
{
    MovingEntity _process;
    public MovingEntity process
    {
        get
        {
            if (_process == null)
                _process = new MovingEntity(this);
            return _process;
        }
    }
    
    public void Begin() => process.Begin();
    public void RefreshPosition(float lerpTime) => process.RefreshPosition(lerpTime);
}