using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Ice")]
public class IceCell : GridCellInfo
{
    [SerializeField, Range(0, 1)] float slideChance = 0.5f;

    public override bool Touch(MovingEntity entity, GridCell cell)
    {
        if (Random.Range(0f, 1f) > slideChance) return false;
        
        var movement = entity.moveDirection; 
        if (movement == Vector3.zero) return false;
        
        var lastPosition = entity.stepPath[^1];
        var nextPosition = lastPosition + movement;
        entity.AddToPath(nextPosition);
        //Debug.Log($"{lastPosition} {movement} {nextPosition} {entity.stepPath.Count}");
        //Debug.Log($"Full path:  {string.Join(", ", entity.stepPath)}"); 
        
        var hitObstacle = entity.CheckForColliders(nextPosition);
        if (hitObstacle) 
        {
            entity.RemoveLastFromPath();
            return false;
        }
        return false;
    }
}
