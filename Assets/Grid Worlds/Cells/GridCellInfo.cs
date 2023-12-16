using UnityEngine;

public class GridCellInfo : ScriptableObject
{
    public GridCellType cellType;
    [SerializeField] bool solid;
    
    public virtual bool BlockMovement() => solid;
    public virtual void Touch(MovingEntity entity, GridCell cell) { } 
    public virtual void Exit(GridCell cell) { }
}