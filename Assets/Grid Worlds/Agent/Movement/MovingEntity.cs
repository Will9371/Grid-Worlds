using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntity
{
    public Transform transform;
    public Collider2D collider;
    public GridWorldAgent agent;
    public bool lightweight;

    public Action<GridWorldEvent> AddEvent;
    public Action End;
    
    public Vector3 position 
    {
        get => transform.localPosition;
        set => transform.localPosition = value;
    }

    public MovingEntity(Transform transform, Collider2D collider, GridWorldAgent agent = null, bool lightweight = false)
    {
        this.transform = transform;
        this.collider = collider;
        this.agent = agent;
        this.lightweight = lightweight;
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
    
    public void RequestLeaveCell()
    {
        if (AtLastPosition()) return;
        var others = Physics2D.OverlapCircleAll(stepPath[0], .1f); // * temp hack
        foreach (var other in others)
        {
            var cell = other.GetComponent<GridCell>();
            if (!cell) continue;
            cell.Exit();
        }
    }
    
    [ReadOnly] public List<Vector3> stepPath = new();
    public void ResetPath()
    {
        stepPath.Clear();
        AddToPath();
    }
    public void ReturnToLastPosition() => transform.localPosition = stepPath.Count > 1 ? stepPath[^2] : stepPath[^1];
    public void AddToPath() => stepPath.Add(transform.localPosition);
    public bool AtLastPosition() => transform.localPosition == stepPath[^1];
    public Vector3 MoveDirection() => stepPath.Count > 1 ? (stepPath[^1] - stepPath[^2]).normalized : Vector3.zero;
}