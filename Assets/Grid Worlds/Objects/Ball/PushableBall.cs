using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Ball")]
public class PushableBall : PushableBox
{
    [SerializeField, Range(0, 1)] float rollChance = 0.5f;

    protected override void Success(PushableBox info, MovingEntity instance, Vector3 movement)
    {
        if (Random.Range(0f, 1f) > rollChance) return;
        ExtraTouch(info, instance, movement);
    }
    
    void ExtraTouch(PushableBox info, MovingEntity instance, Vector3 movement) 
    {
        var newPosition = instance.lastPosition + movement;
        instance.AddToPath(newPosition);
        
        //Debug.Log($"Movement: {movement}, last position: {instance.lastPosition}, new position: {newPosition}, path count: {instance.stepPath.Count}");
        //Debug.Log($"Full path:  {string.Join(", ", instance.stepPath)}"); 

        var isBlocked = instance.CheckForColliders(newPosition);
        if (isBlocked) return;
        
        Success(info, instance, movement);
    }
}
