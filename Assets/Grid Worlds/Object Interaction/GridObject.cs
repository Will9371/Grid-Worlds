using System;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class GridObject : MonoBehaviour
{
    Lookup lookup => Lookup.instance;

    public RandomizePositionOnBegin positioner;
    public ObjectCollider colliderInterface;
    IObservableObject observable;
    
    public GridObjectData data;
    
    void Awake() => positioner.Awake();
    void OnDrawGizmos() => positioner.OnDrawGizmos();

    void OnValidate()
    {
        positioner.transform = transform;
        data = new GridObjectData(this);
        observable = GetComponent<IObservableObject>();
    }
    
    public void SetRandomPosition() => positioner.SetRandomPosition();
    
    
    public int GetObservationCount()
    {
        int result = 3;  // 2 spatial + 1 id dimension
        if (observable != null) result += observable.observationCount;
        return result;
    }
    
    public void AddObservations(VectorSensor sensor) 
    {
        positioner.AddObservations(sensor);
        sensor.AddObservation(lookup.GetObjectIndex(data.touchInfo));
        if (observable != null) observable.AddObservations(sensor);
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
