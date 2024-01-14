using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntity
{
    public Transform transform;
    public Collider2D collider;
    public GridWorldAgent agent;
    MonoBehaviour mono;
    
    Vector3 startPosition;
    public Vector3 position => transform.localPosition;
    bool isAgent => agent != null;
    
    bool isAlive;

    public MovingEntity(Transform transform, Collider2D collider, MonoBehaviour mono, GridWorldAgent agent = null)
    {
        this.transform = transform;
        this.collider = collider;
        this.agent = agent;
        this.mono = mono;
        startPosition = transform.localPosition;
    }
    
    public void Begin() 
    {
        transform.localPosition = startPosition;
        isAlive = true;
        ResetPath();
    }
    
    /// Adds position to stepPath if there are no blocking colliders at that position.
    /// Returns true if the position is blocked, false if it is open.
    public bool AddToPathIfOpen(Vector2 position, bool isSliding)
    {
        if (stepPath.Count > 20)
        {
            Debug.LogError("max path length exceeded");
            return false;
        }
    
        if (!isAlive) return false;
        AddToPath(position);
        
        var overlaps = Physics2D.OverlapCircleAll(position, .1f);
        var isBlocked = CheckForColliders(overlaps);
        if (isBlocked) RemoveLastFromPath();
        
        TouchPosition(overlaps, isSliding, isBlocked);
        return isBlocked;
    }
    
    bool CheckForColliders(Collider2D[] overlaps)
    {
        foreach (var other in overlaps)
        {
            if (!other || other == collider) continue;
            if (IsBlocker(other)) return true;
        }
            
        return false;
    }
    
    bool IsBlocker(Collider2D other)
    {
        var gridObject = other.GetComponent<ObjectCollider>();
        if (gridObject) return gridObject.BlockMovement(isAgent);
        
        var cell = other.GetComponent<GridCell>();
        if (cell) return cell.BlockMovement();
        
        var agent = other.transform.parent.GetComponent<GridWorldAgent>();
        if (agent) return true;
        
        return false;
    }
    
    void TouchPosition(Collider2D[] others, bool isSliding, bool isBlocked)
    {
        foreach (var other in others)
        {
            // Don't interact with invalid colliders or self
            if (!other || other == collider) continue;
            
            // Prevent sliding objects from moving other objects (or they will appear to move at the same time)
            if (!isSliding) TouchObject(other);
            
            // Only interact with cells when actually stepping on them
            if (!isBlocked) TouchCell(other);
        }
    }
    
    void TouchObject(Collider2D other)
    {
        var gridObject = other.GetComponent<ObjectCollider>();
        if (gridObject) gridObject.Touch(this);
    }
    
    void TouchCell(Collider2D other)
    {
        var cell = other.GetComponent<GridCell>();
        if (cell) cell.Touch(this);        
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
    
    public void RefreshPosition(float lerpTime)
    {
        if (stepPath.Count == 0 || !mono.isActiveAndEnabled) return;
        if (lerpTime <= 0f)
        {
            transform.localPosition = lastPosition;
            ResetPath();            
        }
        else
            mono.StartCoroutine(SmoothMove(lerpTime));
    }
    
    IEnumerator SmoothMove(float lerpTime)
    {
        if (stepPath.Count <= 1)
        {
            transform.localPosition = lastPosition;
            ResetPath();
            yield break;
        }
        
        var segmentDuration = lerpTime/(stepPath.Count - 1);
        for (int i = 0; i < stepPath.Count - 1; i++)
        {
            var startTime = Time.time;
            while (Time.time - startTime < segmentDuration)
            {
                var percent = (Time.time - startTime)/segmentDuration;
                
                if (i < 0 || i >= stepPath.Count || i + 1 < 0 || i + 1 >= stepPath.Count)
                    Debug.LogError($"{i}, {stepPath.Count}");   // Error (line 156) with i=1
                
                transform.localPosition = Vector3.Lerp(stepPath[i], stepPath[i+1], percent);
                
                yield return null;
            }            
        }
        transform.localPosition = lastPosition;
        ResetPath();
        
        if (!isAlive)
        {
            if (isAgent) agent.End();
            else mono.gameObject.SetActive(false);
        }
    }
    
    void ResetPath()
    {
        stepPath.Clear();
        AddCurrentPositionToPath();
        moveDirection = Vector3.zero;
    }
    
    public Vector3 moveDirection;
    
    public void AddEvent(GridWorldEvent id)
    {
        if (agent) agent.AddEvent(id);
    }
    
    public void Die()
    {
        //if (agent) agent.End();
        isAlive = false;
    }
}