using UnityEngine;

public enum GridCellType { Empty, Wall, Lava, Ice, ActivatedWall, Bumper }

public class GridCell : MonoBehaviour
{
    Lookup lookup => Lookup.instance;
    
    #region Dependencies
    DisplayLastResult displayLastResult
    {
        get
        {
            if (!_displayLastResult)
                _displayLastResult = GetComponent<DisplayLastResult>();
            return _displayLastResult;
        }
    }
    DisplayLastResult _displayLastResult;
    BoxCollider2D boxCollider
    {
         get
         {
            if (!_boxCollider)
                _boxCollider = GetComponent<BoxCollider2D>();
                
            return _boxCollider;
         }
    }
    BoxCollider2D _boxCollider;
    public SpriteRenderer rend
    {
        get
        {
            if (!_rend)
                _rend = GetComponent<SpriteRenderer>();
            return _rend;
        }
    }
    SpriteRenderer _rend;
    #endregion;
    
    [Header("Settings")]
    public GridCellType cellType;
    [HideInInspector]
    public GridCellInfo interaction;
    public GridCellInfo instanceInfo;
    GridCellInfo info => cellSettings.setPerInstance ? instanceInfo : interaction;
    
    GridCellSettings cellSettings;
    
    public float x => transform.localPosition.x;
    public float y => transform.localPosition.y;
    public float typeIndex => (float)cellType;
    
    void OnValidate()
    {
        SetData(cellType);
        if (cellSettings.setPerInstance && instanceInfo) instanceInfo.LateValidate(this);
    }

    public void SetData(GridCellType cellType)
    {
        this.cellType = cellType;
        cellSettings = lookup.GetGridCellSettings(cellType);
        interaction = lookup.GetInteractable(cellType);

        displayLastResult.enabled = cellSettings.displayResultColorOnEpisodeEnd;
        boxCollider.enabled = cellSettings.hasCollider;
        rend.color = cellSettings.color;
        
        if (cellType != GridCellType.Bumper)
            instanceInfo = null;
        if (!instanceInfo)
            rend.sprite = cellSettings.sprite;
    }
    
    public void AddObservations(AgentObservations sensor)
    {
        sensor.Add("x", x);
        sensor.Add("y", y);
        sensor.Add($"{cellType.ToString()}", typeIndex);
    }
    
    public bool BlockMovement() => info.BlockMovement();
    public void Touch(MovingEntity entity) => info.Touch(entity, this);
    
    
    public void Exit() => info.Exit(this);
    
    GridCellType startType;
    void Awake() => startType = cellType;
    public void BeginEpisode() => SetData(startType);
}
