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
    
    public Vector3 position => transform.localPosition;

    public MovingEntity(Transform transform, Collider2D collider, GridWorldAgent agent = null, bool lightweight = false)
    {
        this.transform = transform;
        this.collider = collider;
        this.agent = agent;
        this.lightweight = lightweight;
    }
    
    public bool CheckForColliders(Vector3 position)
    {
        var others = Physics2D.OverlapCircleAll(position, .1f);
        var isBlocked = false;
        
        foreach (var other in others)
        {
            if (other == collider) continue;
            if (OnTouch2D(other)) isBlocked = true;
        }
        
        return isBlocked;
    }
    
    bool OnTouch2D(Collider2D other)
    {
        var isBlocked = false;
    
        var cell = other.GetComponent<GridCell>();
        if (cell) if (cell.Touch(this)) isBlocked = true;

        var gridObject = other.GetComponent<ObjectCollider>();
        if (gridObject) if (gridObject.Touch(this)) isBlocked = true;
        
        return isBlocked;
    }
    
    public void LeaveCell(Vector3 position)
    {
        var others = Physics2D.OverlapCircleAll(position, .1f); // ERROR: false positive for activated wall when pushing barrel
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
    public void AddToPath() => stepPath.Add(transform.localPosition);
    public void AddToPath(Vector3 value) => stepPath.Add(value);
    public Vector3 moveDirection;
}