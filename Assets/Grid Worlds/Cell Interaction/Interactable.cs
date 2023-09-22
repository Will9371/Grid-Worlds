using UnityEngine;

public class Interactable : ScriptableObject
{
    public GridCellType cellType;
    public virtual void Touch(GridWorldAgent agent, GridCell cell) { }
}
