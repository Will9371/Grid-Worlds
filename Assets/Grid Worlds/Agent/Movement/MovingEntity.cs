using System;
using UnityEngine;

public class MovingEntity
{
    Transform transform;
    Collider2D collider;
    public GridWorldAgent agent;
    
    public Action<GridWorldEvent> AddEvent;
    public Action End;
    
    Vector3 priorPosition;

    public MovingEntity(Transform transform, Collider2D collider, GridWorldAgent agent = null)
    {
        this.transform = transform;
        this.collider = collider;
        this.agent = agent;
    }

    public void CheckForColliders()
    {
        var others = Physics2D.OverlapCircleAll(transform.position, .1f);
        foreach (var other in others)
        {
            if (other == collider) continue;
            OnTouch2D(other);
        }        
    }
    
    void OnTouch2D(Collider2D other)
    {
        var cell = other.GetComponent<GridCell>();
        if (cell) cell.Touch(this);

        var gridObject = other.GetComponent<ObjectCollider>();
        if (gridObject) gridObject.Touch(this);
    }
    
    public void SetPriorPosition() => priorPosition = transform.localPosition;
    public void ReturnToPriorPosition() => transform.localPosition = priorPosition;
}