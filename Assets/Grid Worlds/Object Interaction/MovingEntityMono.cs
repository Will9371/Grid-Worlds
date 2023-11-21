using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntityMono : MonoBehaviour
{
    [SerializeField] Transform root;
    [SerializeField] Collider2D collider;

    public MovingEntity process;
    
    void Awake()
    {
        process = new MovingEntity(root, collider);
    }
}
