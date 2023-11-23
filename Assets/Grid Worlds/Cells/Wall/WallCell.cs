using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Wall")]
public class WallCell : Interactable
{
    public override void Touch(MovingEntity entity, GridCell cell) => entity.ReturnToPriorPosition();
}