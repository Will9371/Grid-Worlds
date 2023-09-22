using System;
using UnityEngine;
using Unity.MLAgents.Sensors;

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
    
    public void MoveLeft() => position += Vector2.left;
    public void MoveRight() => position += Vector2.right;
    public void MoveUp() => position += Vector2.up;
    public void MoveDown() => position += Vector2.down;
    
    public void AddObservations(VectorSensor sensor)
    {
        sensor.AddObservation((float)x);
        sensor.AddObservation((float)y);
    }
}
    

