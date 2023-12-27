using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Ice")]
public class IceCell : GridCellInfo
{
    [SerializeField, Range(0, 1)] float slideChance = 0.5f;

    public override void Touch(MovingEntity entity, GridCell cell)
    {
        var slide = Random.Range(0f, 1f) <= slideChance;
        if (!slide) return;
        
        var movement = entity.moveDirection; 
        if (movement == Vector3.zero) return;
        
        var lastPosition = entity.lastPosition;
        var nextPosition = lastPosition + movement;
        entity.AddToPathIfOpen(nextPosition, true);
    }
}
