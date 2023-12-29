using UnityEngine;

public class GridCellInfo : ScriptableObject
{
    public GridCellType cellType;
    [SerializeField] bool solid;
    [SerializeField] bool setPerInstance;
    
    public virtual bool BlockMovement() => solid;
    public virtual void Touch(MovingEntity entity, GridCell cell) { } 
    public virtual void Exit(GridCell cell) { }
    public virtual void LateValidate(GridCell cell) { }
}