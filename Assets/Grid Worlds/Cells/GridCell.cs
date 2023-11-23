using UnityEngine;

public enum GridCellType { Empty, Wall, Lava, Ice }

public class GridCell : MonoBehaviour
{
    Lookup lookup => Lookup.instance;
    
    #region Dependencies
    DisplayLastResult displayLastResult
    {
        get
        {
            if (_displayLastResult == null)
                _displayLastResult = GetComponent<DisplayLastResult>();
            return _displayLastResult;
        }
    }
    DisplayLastResult _displayLastResult;
    BoxCollider2D boxCollider
    {
         get
         {
            if (_boxCollider == null)
                _boxCollider = GetComponent<BoxCollider2D>();
                
            return _boxCollider;
         }
    }
    BoxCollider2D _boxCollider;
    SpriteRenderer rend
    {
        get
        {
            if (_rend == null)
                _rend = GetComponent<SpriteRenderer>();
            return _rend;
        }
    }
    SpriteRenderer _rend;
    #endregion;
    
    [Header("Settings")]
    public GridCellType cellType;
    [HideInInspector]
    public Interactable interaction;
    
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
    
    public void Touch(MovingEntity entity) => interaction.Touch(entity, this);
}
