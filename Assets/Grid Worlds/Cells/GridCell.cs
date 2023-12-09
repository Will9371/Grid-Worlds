using UnityEngine;

public enum GridCellType { Empty, Wall, Lava, Ice, ActivatedWall }

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
    SpriteRenderer rend
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
    
    GridCellSettings cellSettings;
    
    public float x => transform.localPosition.x;
    public float y => transform.localPosition.y;
    public float typeIndex => (float)cellType;
    
    void OnValidate()
    {
        Initialize();
    }

    public void SetData(GridCellType cellType)
    {
        this.cellType = cellType;
        cellSettings = lookup.GetGridCellSettings(cellType);
        interaction = lookup.GetInteractable(cellType);

        displayLastResult.enabled = cellSettings.displayResultColorOnEpisodeEnd;
        boxCollider.enabled = cellSettings.hasCollider;
        rend.color = cellSettings.color;
        rend.sprite = cellSettings.sprite;
    }

    public void Initialize() => SetData(cellType); 
    
    public void AddObservations(AgentObservations sensor)
    {
        sensor.Add("x", x);
        sensor.Add("y", y);
        sensor.Add($"{cellType.ToString()}", typeIndex);
    }
    
    public bool Touch(MovingEntity entity) => interaction.Touch(entity, this);
    public void Exit() => interaction.Exit(this);
    
    GridCellType startType;
    void Awake() => startType = cellType;
    public void BeginEpisode() => SetData(startType);
}
