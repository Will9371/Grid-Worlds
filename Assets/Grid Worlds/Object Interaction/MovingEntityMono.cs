using UnityEngine;
using UnityEngine.Events;

public class MovingEntityMono : MonoBehaviour
{
    [SerializeField] Transform root;
    [SerializeField] Collider2D collider;
    [SerializeField] UnityEvent onEnd;

    public MovingEntity process;
    
    void Awake()
    {
        process = new MovingEntity(root, collider);
        process.End = End;
    }
    
    void End() => onEnd.Invoke();
}
