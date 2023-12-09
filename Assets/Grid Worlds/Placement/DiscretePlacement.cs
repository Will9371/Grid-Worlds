using System;
using UnityEngine;

[Serializable]
public class DiscretePlacement
{
    public Transform transform;

    public DiscretePlacement(Transform transform)
    {
        this.transform = transform;
    }

    public Vector2 IntPosition(Vector2 value) => new (Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));

    public int x => Mathf.RoundToInt(transform.localPosition.x);
    public int y => Mathf.RoundToInt(transform.localPosition.y);
    public Vector2 position
    {
        get => new (x, y);
        set => transform.localPosition = IntPosition(value);
    }
    
    public Vector3 Left() => position + Vector2.left;
    public Vector3 Right() => position + Vector2.right;
    public Vector3 Up() => position + Vector2.up;
    public Vector3 Down() => position + Vector2.down;
    public Vector3 Zero() => position;
    
    public void AddObservations(AgentObservations sensor)
    {
        sensor.Add("x", x);
        sensor.Add("y", y);
    }
}
    

