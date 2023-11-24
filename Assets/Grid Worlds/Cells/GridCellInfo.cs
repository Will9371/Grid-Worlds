using UnityEngine;

public class GridCellInfo : ScriptableObject
{
    public GridCellType cellType;
    public virtual void Touch(MovingEntity entity, GridCell cell) { }
    public virtual void Exit(GridCell cell) { }
}