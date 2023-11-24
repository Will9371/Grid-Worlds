using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Activated Wall")]
public class ActivatedWallCell : GridCellInfo
{
    public GridCellInfo typeOnExit;
    public override void Exit(GridCell cell) => cell.SetData(typeOnExit.cellType);
}