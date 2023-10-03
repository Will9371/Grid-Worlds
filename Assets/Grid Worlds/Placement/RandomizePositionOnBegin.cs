using System;
using UnityEngine;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

[Serializable]
public class RandomizePositionOnBegin
{
    public Vector2 xRange;
    public Vector2 yRange;
    
    public int xMin => Mathf.RoundToInt(xRange.x + center.x);
    public int xMax => Mathf.RoundToInt(xRange.y + center.x);
    public int yMin => Mathf.RoundToInt(yRange.x + center.y);
    public int yMax => Mathf.RoundToInt(yRange.y + center.y);
    
    //[HideInInspector]
    public Vector2 center;
    //[HideInInspector] 
    public Transform transform;
    
    DiscretePlacement location;

    public Vector2 GetRandomPlacement => new (Random.Range(xMin, xMax), Random.Range(yMin, yMax));
    
    public void SetRandomPosition(DiscretePlacement placement) => placement.position = GetRandomPlacement;
    public void SetRandomPosition(Transform transform) => SetRandomPosition(new DiscretePlacement(transform));
    public void SetRandomPosition() => SetRandomPosition(location);
    
    public void SetPosition(Vector2 value) => transform.localPosition = location.IntPosition(value);
    
    [Header("Editor")]
    public Color gizmoColor = Color.black;
    public float gizmoRadius = 0.25f;
    
    public void OnDrawGizmos()
    {
        if (!transform) return;

        if (xRange.x == 0 && xRange.y == 0 && yRange.x == 0 && yRange.y == 0)
            return;
        
        SetCenter();
        Gizmos.color = gizmoColor;
        
        for (int y = yMin; y <= yMax; y++)
            for (int x = xMin; x <= xMax; x++)
                Gizmos.DrawSphere(new Vector3(x, y, 0f), gizmoRadius);
    }
    
    public void Awake()
    {
        SetCenter();
        location = new DiscretePlacement(transform);
    }
    
    public void AddObservations(VectorSensor sensor) => location.AddObservations(sensor);
    
    public void SetCenter() => center = 
        new Vector2(Mathf.RoundToInt(transform.localPosition.x), Mathf.RoundToInt(transform.localPosition.y));
}
