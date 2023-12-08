using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Box")]
public class PushableBox : GridObjectInfo
{
    public override void Touch(MovingEntity source, GameObject self) 
    {
        var pushable = self.GetComponent<MovingEntityMono>();
        if (!pushable) return;
        
        pushable.process.ResetPath();
        
        var movement = Vector3.zero;
        
        // Lighter objects (e.g. ball) cannot push heavier objects (e.g. box)
        if (!source.lightweight) 
        {
            movement = source.MoveDirection();
            pushable.process.position += movement;
            pushable.process.AddToPath();
        }
        
        pushable.process.CheckForColliders();
        source.ReturnToLastPosition();
        
        if (!pushable.process.AtLastPosition())
            Success(this, pushable.process, movement);
    }
    
    public void Touch(PushableBox info, MovingEntity instance, Vector3 movement) 
    {
        instance.ResetPath();
        instance.position += movement;
        instance.AddToPath();
        instance.CheckForColliders();
        
        if (!instance.AtLastPosition())
            Success(info, instance, movement);
    }
    
    protected virtual void Success(PushableBox info, MovingEntity instance, Vector3 movement) { }
}
