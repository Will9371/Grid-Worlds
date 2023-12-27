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
        var isBlocked = instance.AddToPathIfOpen(newPosition, false);
        if (!isBlocked) Success(info, instance, movement);
    }
}
