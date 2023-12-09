using UnityEngine;

public class GridCellInfo : ScriptableObject
{
    public GridCellType cellType;
    public virtual bool Touch(MovingEntity entity, GridCell cell) => false; 
    public virtual void Exit(GridCell cell) { }
}