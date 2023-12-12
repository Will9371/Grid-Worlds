using System.Collections.Generic;
using UnityEngine;

public class MovingEntity
{
    public Transform transform;
    public Collider2D collider;
    public GridWorldAgent agent;
    public bool lightweight;
    
    Vector3 startPosition;
    public Vector3 position => transform.localPosition;

    public MovingEntity(Transform transform, Collider2D collider, GridWorldAgent agent = null, bool lightweight = false)
    {
        this.transform = transform;
        this.collider = collider;
        this.agent = agent;
        this.lightweight = lightweight;
        startPosition = transform.localPosition;
    }
    
    public void Begin() 
    {
        transform.localPosition = startPosition;
        ResetPath();
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
        
        if (isBlocked) 
            RemoveLastFromPath();
            
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
        var others = Physics2D.OverlapCircleAll(position, .1f); // ERROR (verify): false positive for activated wall when pushing barrel
        foreach (var other in others)
        {
            var cell = other.GetComponent<GridCell>();
            if (!cell) continue;
            cell.Exit();
        }
    }
    
    [ReadOnly] public List<Vector3> stepPath = new();
    public void AddCurrentPositionToPath() => stepPath.Add(transform.localPosition);
    public void AddToPath(Vector3 value) => stepPath.Add(value);
    public void RemoveLastFromPath() => stepPath.RemoveAt(stepPath.Count - 1);
    public Vector3 lastPosition => stepPath[^1];
    
    public void RefreshPosition()
    {
        if (stepPath.Count == 0) return;
        transform.localPosition = lastPosition;
        ResetPath();
    }
    void ResetPath()
    {
        stepPath.Clear();
        AddCurrentPositionToPath();
    }
    
    public Vector3 moveDirection;
    
    public void AddEvent(GridWorldEvent id)
    {
        if (agent) agent.AddEvent(id);
    }
    
    public void Die()
    {
        if (agent) agent.End();
    }
}