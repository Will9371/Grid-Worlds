/*
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RandomDiscretePlacement
{
    public Vector2 xRange;
    public Vector2 yRange;
    
    public int xMin => Mathf.RoundToInt(xRange.x + center.x);
    public int xMax => Mathf.RoundToInt(xRange.y + center.x);
    public int yMin => Mathf.RoundToInt(yRange.x + center.y);
    public int yMax => Mathf.RoundToInt(yRange.y + center.y);
    
    [HideInInspector]
    public Vector2 center;
    
    public Vector2 GetRandomPlacement => new (Random.Range(xMin, xMax), Random.Range(yMin, yMax));
    
    public void SetRandomPlacement(DiscretePlacement placement) => placement.position = GetRandomPlacement;
    public void SetRandomPlacement(Transform transform) => SetRandomPlacement(new DiscretePlacement(transform));
    
    [Header("Editor")]
    [SerializeField] Color gizmoColor = Color.black;
    [SerializeField] float gizmoRadius = 0.25f;
    
    public void OnDrawGizmos()
    {
        if (xMin == 0 && xMax == 0 && yMin == 0 && yMax == 0)
            return;
        
        Gizmos.color = gizmoColor;
        
        for (int y = yMin; y <= yMax; y++)
            for (int x = xMin; x <= xMax; x++)
                Gizmos.DrawSphere(new Vector3(x, y, 0f), gizmoRadius);
    }
}
*/
