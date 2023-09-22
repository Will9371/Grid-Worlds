using UnityEngine;
using Unity.MLAgents.Sensors;

public enum GridCellType { Empty, Wall, Lava }

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
        SetCellData(cellType);
    }

    public void SetCellData(GridCellType cellType)
    {
        this.cellType = cellType;
        cellSettings = lookup.GetGridCellSettings(cellType);
        interaction = lookup.GetInteractable(cellType);

        displayLastResult.enabled = cellSettings.displayResultColorOnEpisodeEnd;
        boxCollider.enabled = cellSettings.hasCollider;
        rend.color = cellSettings.color;
    }

    public void Initialize() { SetCellData(cellType); }
    
    public void AddObservations(VectorSensor sensor)
    {
        sensor.AddObservation(x);
        sensor.AddObservation(y);
        sensor.AddObservation(typeIndex);
    }
    
    public void Touch(GridWorldAgent agent) => interaction.Touch(agent, this);
}

//if (environmentSettings.values == null)
//   return;
    
//foreach (var item in environmentSettings.values)
//    if (item.id == cellType)
//        cellSettings = item; 
