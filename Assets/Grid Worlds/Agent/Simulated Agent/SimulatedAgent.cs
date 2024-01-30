using System.Collections.Generic;
using UnityEngine;

public class SimulatedAgent : MonoBehaviour
{
    MovingEntity _movement;
    public MovingEntity movement
    {
        get
        {
            if (_movement == null)
                _movement = new MovingEntity(this);
            return _movement;
        }
    }
    
    public void FollowPath(List<Vector3> path, float delay, float delayBuffer)
    {
        movement.CopyPath(path);
        movement.RefreshPosition(delay);
        Destroy(gameObject, delay + delayBuffer);
    }
}
