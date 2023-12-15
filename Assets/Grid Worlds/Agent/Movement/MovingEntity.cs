using System.Collections.Generic;
using UnityEngine;

public class MovingEntity
{
    public Transform transform;
    public Collider2D collider;
    public GridWorldAgent agent;
    //public bool lightweight;
    
    Vector3 startPosition;
    public Vector3 position => transform.localPosition;

    public MovingEntity(Transform transform, Collider2D collider, GridWorldAgent agent = null)
    {
        this.transform = transform;
        this.collider = collider;
        this.agent = agent;
        //this.lightweight = lightweight;
        startPosition = transform.localPosition;
    }
    
    public void Begin() 
    {
        transform.localPosition = startPosition;
        ResetPath();
    }
    
    /// Adds position to stepPath if there are no blocking colliders at that position.
    /// Returns true if the position is blocked, false if it is open.
    public bool AddToPathIfOpen(Vector3 position)
    {
        AddToPath(position);
        var isBlocked = CheckForColliders(position);
        //Debug.Log($"{transform.name} blocked = {isBlocked} at time {Time.time}.  Checking position {position}, from position {transform.position}");
        if (isBlocked) RemoveLastFromPath();
        return isBlocked;
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
    
    public void RequestLeaveCell()
    {
        if (atLastPosition) return;
        
        var others = Physics2D.OverlapCircleAll(position, .1f);
        foreach (var other in others)
        {
            var cell = other.GetComponent<GridCell>();
            if (!cell) continue;
            cell.Exit();
        }
    }
    
    [ReadOnly] public List<Vector3> stepPath = new();
    public void AddCurrentPositionToPath() => stepPath.Add(transform.localPosition);
    void AddToPath(Vector3 value) => stepPath.Add(value);
    void RemoveLastFromPath() => stepPath.RemoveAt(stepPath.Count - 1);
    public Vector3 lastPosition => stepPath[^1];
    public bool atLastPosition => transform.localPosition == lastPosition;
    
    // TBD: Lerp movement
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