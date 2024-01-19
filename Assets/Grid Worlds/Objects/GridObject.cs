using System;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    GridWorldEnvironment _environment;
    public GridWorldEnvironment environment
    {
        get => _environment;
        set
        {
            _environment = value;
            var moveMono = GetComponent<MovingEntityMono>();
            if (moveMono) moveMono.process.environment = value;
        }
    }

    [Header("Settings")]
    public new string name;
    [SerializeField] bool setNameFromInfo = true;
    public GridObjectInfo info;
    public WatchTransformInEditor editPosition;
    [VectorLabels("Min", "Max")]
    public Vector2 xRange;
    [VectorLabels("Min", "Max")]
    public Vector2 yRange;
    [SerializeField] bool overrideColor;
    public Color color = Color.yellow;
    [Range(0, 1)] public float gizmoRadius = 0.25f;
    
    [Header("References")]
    //public ObjectCollider colliderInterface;
    public SpriteRenderer rend;
    IObservableObject observable;
    
    [ReadOnly] public RandomizePositionOnBegin positioner = new();
    [ReadOnly] public GridObjectData data;
    
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
        //colliderInterface.info = info;
        editPosition.OnValidate(this, SetCenter);

        positioner.xRange = xRange;
        positioner.yRange = yRange;
        positioner.gizmoColor = color;
        positioner.gizmoRadius = gizmoRadius;
        positioner.transform = transform;
        
        if (!rend) rend = GetComponent<SpriteRenderer>();
        if (!overrideColor) rend.color = data.color;

        data = new GridObjectData(this);
        observable = GetComponent<IObservableObject>(); 
    }
    
    void SetCenter(Vector3 value) 
    {
        positioner.SetCenter();
        data.position = positioner.center;
    }

    public void Initialize(GridObjectData data)
    {
        this.data = data;
        info = data.touchInfo;
        this.name = data.name;
        gameObject.name = data.name;
        gameObject.SetActive(!data.hide);
        //Debug.Log($"Initializing {data.name}, active = {!data.hide}");
        transform.localPosition = new Vector3(data.position.x, data.position.y, 0f);
        positioner.transform = transform;
        positioner.xRange = data.xPlaceRange;
        positioner.yRange = data.yPlaceRange;
        positioner.gizmoRadius = data.gizmoRadius;
        positioner.gizmoColor = data.color;
        if (!overrideColor) rend.color = data.color;
    }
    
    public void SetRandomPosition() => positioner.SetRandomPosition();
    
    public bool BlockMovement(bool isAgent) => info.BlockMovement(isAgent);
    public void Touch(MovingEntity entity) => info.Touch(entity, gameObject);
    
    public int GetObservationCount()
    {
        int result = 3;  // 2 spatial + 1 id dimension
        if (observable != null) result += observable.observationCount;
        return result;
    }
    
    public void AddObservations(AgentObservations sensor) 
    {
        sensor.Add(Statics.PositionString(transform), "", data.name);
        observable?.AddObservations(sensor);
    }
    
    public void BeginEpisode()
    {
        //Debug.Log($"GridObject.BeginEpisode({data.hide})", gameObject);
        gameObject.SetActive(!data.hide);
        movement = GetComponent<MovingEntityMono>();
        if (movement) movement.Begin();
        SetRandomPosition();
    }
    
    MovingEntityMono movement;
    
    public void RefreshPosition(float lerpTime)
    {
        if (!movement) return;
        movement.RefreshPosition(lerpTime);
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
    public bool hide;
    
    public GridObjectData(GridObject source)
    {
        name = source.name;
        position = source.positioner.center;
        xPlaceRange = source.xRange;
        yPlaceRange = source.yRange;
        touchInfo = source.info;
        color = source.color;
        gizmoRadius = source.gizmoRadius;
        hide = !source.gameObject.activeSelf;
    }
}
