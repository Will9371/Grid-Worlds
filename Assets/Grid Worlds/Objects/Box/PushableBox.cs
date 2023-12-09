using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Box")]
public class PushableBox : GridObjectInfo
{
    public override bool Touch(MovingEntity source, GameObject self) 
    {
        var pushable = self.GetComponent<MovingEntityMono>();
        if (!pushable) return true;
        
        pushable.process.ResetPath();
        var nextPosition = pushable.process.position;
        var movement = Vector3.zero;
        
        // Lighter objects (e.g. ball) cannot push heavier objects (e.g. box)
        bool success = !source.lightweight;
        if (!success) return true;
        
        movement = source.moveDirection;
        nextPosition += movement;
        success = !pushable.process.CheckForColliders(nextPosition);
        
        if (!success) return true;
        {
            pushable.process.AddToPath();
            pushable.transform.localPosition = nextPosition;
            Success(this, pushable.process, movement);
        }
        return true;
    }
    
    public void Touch(PushableBox info, MovingEntity instance, Vector3 movement) 
    {
        var newPosition = instance.position + movement;
        var success = !instance.CheckForColliders(newPosition);
        if (!success) return;
        
        instance.AddToPath();
        Success(info, instance, movement);
    }
    
    protected virtual void Success(PushableBox info, MovingEntity instance, Vector3 movement) { }
}
