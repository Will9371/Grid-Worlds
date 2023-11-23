using UnityEngine;

public class MovingEntityMono : MonoBehaviour
{
    [SerializeField] Transform root;
    [SerializeField] Collider2D collider;
    [SerializeField] bool lightweight;

    public MovingEntity process;
    
    void Awake()
    {
        process = new MovingEntity(root, collider, null, lightweight);
    }
}
