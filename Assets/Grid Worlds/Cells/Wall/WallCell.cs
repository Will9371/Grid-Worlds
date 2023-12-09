using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Wall")]
public class WallCell : GridCellInfo
{
    public override bool Touch(MovingEntity entity, GridCell cell) => true; 
}