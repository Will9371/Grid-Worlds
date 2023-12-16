using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Ice")]
public class IceCell : GridCellInfo
{
    [SerializeField, Range(0, 1)] float slideChance = 0.5f;

    public override void Touch(MovingEntity entity, GridCell cell)
    {
        if (Random.Range(0f, 1f) > slideChance) return;
        
        var movement = entity.moveDirection; 
        if (movement == Vector3.zero) return;
        
        var lastPosition = entity.lastPosition;
        var nextPosition = lastPosition + movement;
        var isBlocked = entity.AddToPathIfOpen(nextPosition);   // ERROR: triggers multiple calls (unknown order), should cancel if any are false on this step.
        // ...
    }
}
