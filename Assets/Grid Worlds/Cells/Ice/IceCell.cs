using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Ice")]
public class IceCell : GridCellInfo
{
    [SerializeField, Range(0, 1)] float slideChance = 0.5f;

    public override void Touch(MovingEntity entity, GridCell cell)
    {
        if (Random.Range(0f, 1f) > slideChance) return;
        
        var movement = entity.MoveDirection();
        if (movement == Vector3.zero) return;
        
        entity.transform.localPosition += movement;
        entity.AddToPath();
        entity.CheckForColliders();
    }
}
