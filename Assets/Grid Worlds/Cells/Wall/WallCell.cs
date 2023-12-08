using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Wall")]
public class WallCell : GridCellInfo
{
    public override void Touch(MovingEntity entity, GridCell cell) => entity.ReturnToLastPosition();
}