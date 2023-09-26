using System;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class GridObject : MonoBehaviour
{
    Lookup lookup => Lookup.instance;

    public RandomizePositionOnBegin positioner;
    public ObjectCollider colliderInterface;
    
    public GridObjectData data;
    
    void Awake() => positioner.Awake();
    void OnDrawGizmos() => positioner.OnDrawGizmos();

    void OnValidate()
    {
        positioner.transform = transform;
        data = new GridObjectData(this);
    }
    
    public void SetRandomPosition() => positioner.SetRandomPosition();
    
    public void AddObservations(VectorSensor sensor) 
    {
        positioner.AddObservations(sensor);
        sensor.AddObservation(lookup.GetObjectIndex(data.touchInfo));
    }
}

[Serializable]
public struct GridObjectData
{
    public Vector2 position;
    public Vector2 xPlaceRange;
    public Vector2 yPlaceRange;
    public GridObjectInfo touchInfo;
    
    public GridObjectData(GridObject source)
    {
        position = source.positioner.center;
        xPlaceRange = source.positioner.xRange;
        yPlaceRange = source.positioner.yRange;
        touchInfo = source.colliderInterface.info;
    }
}
