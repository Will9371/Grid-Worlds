using UnityEngine;

/// Consider rename
[CreateAssetMenu(menuName = "Grid Worlds/Object/Box")]
public class PushableBox : GridObjectInfo
{
    public override void Touch(MovingEntity source, GameObject self) 
    {
        var pushableMono = self.GetComponent<MovingEntityMono>();
        if (!pushableMono) return;
        
        var pushable = pushableMono.process;
        var nextPosition = pushable.position;
        
        var movement = source.moveDirection;
        pushable.moveDirection = movement;
        nextPosition += movement;
        
        var isBlocked = pushable.AddToPathIfOpen(nextPosition, false);
        if (isBlocked) return;
        
        Success(this, pushable, movement);
    }

    protected virtual void Success(PushableBox info, MovingEntity instance, Vector3 movement) { }
}
