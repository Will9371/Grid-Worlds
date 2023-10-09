using System;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class GridObject : MonoBehaviour
{
    Lookup lookup => Lookup.instance;

    [Header("Settings")]
    public new string name;
    [SerializeField] bool setNameFromInfo = true;
    public GridObjectInfo info;
    public WatchTransformInEditor editPosition;
    public Vector2 xRange;
    public Vector2 yRange;
    public Color color = Color.yellow;
    [Range(0, 1)] public float gizmoRadius = 0.25f;
    
    [Header("References")]
    public ObjectCollider colliderInterface;
    public SpriteRenderer rend;
    IObservableObject observable;
    
    //[Header("Debug")]
    //public Vector2 center;
    [HideInInspector]
    public RandomizePositionOnBegin positioner = new();
    [HideInInspector]
    public GridObjectData data;
    
    void Awake() => positioner.Awake();
    void OnDrawGizmos() => positioner.OnDrawGizmos();

    public void OnValidate() 
    {
        if (setNameFromInfo && info)
        {
            name = info.name;
            setNameFromInfo = false;
        }
    
        gameObject.name = name;
        colliderInterface.info = info;
        editPosition.OnValidate(this, SetCenter);

        positioner.xRange = xRange;
        positioner.yRange = yRange;
        positioner.gizmoColor = color;
        positioner.gizmoRadius = gizmoRadius;
        positioner.transform = transform;
        //positioner.center = center;
        
        if (!rend) rend = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rend.color = color;
        
        data = new GridObjectData(this);
        observable = GetComponent<IObservableObject>(); 
    }
    
    void SetCenter(Vector3 value) 
    {
        positioner.SetCenter();
        data.position = positioner.center;
        //positioner.center = new Vector2(Mathf.RoundToInt(transform.localPosition.x), Mathf.RoundToInt(transform.localPosition.y));
    }

    public void Initialize(GridObjectData data)
    {
        this.data = data;
        info = data.touchInfo;
        this.name = data.name;
        gameObject.name = data.name;
        transform.localPosition = new Vector3(data.position.x, data.position.y, 0f);
        positioner.transform = transform;
        positioner.xRange = data.xPlaceRange;
        positioner.yRange = data.yPlaceRange;
        positioner.gizmoRadius = data.gizmoRadius;
        positioner.gizmoColor = data.color;
        rend.color = data.color;
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
    public string name;
    public GridObjectInfo touchInfo;
    public Vector2 position;
    public Vector2 xPlaceRange;
    public Vector2 yPlaceRange;
    public Color color;
    [Range(0,1)] public float gizmoRadius;
    
    public GridObjectData(GridObject source)
    {
        name = source.name;
        position = source.positioner.center;
        xPlaceRange = source.xRange;
        yPlaceRange = source.yRange;
        touchInfo = source.info;
        color = source.color;
        gizmoRadius = source.gizmoRadius;
    }
}
