using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Box")]
public class PushableBox : GridObjectInfo
{
    public override bool Touch(MovingEntity source, GameObject self) 
    {
        var pushableMono = self.GetComponent<MovingEntityMono>();
        if (!pushableMono) return true;
        var pushable = pushableMono.process;
        
        var nextPosition = pushable.position;
        var movement = Vector3.zero;
        
        // Lighter objects (e.g. ball) cannot push heavier objects (e.g. box)
        if (source.lightweight) return true;
        
        movement = source.moveDirection;
        nextPosition += movement;
        pushable.AddToPath(nextPosition);
        
        var hitObstacle = pushable.CheckForColliders(nextPosition);
        
        Debug.Log($"{movement} {nextPosition} {pushable.stepPath.Count}");
        Debug.Log($"Full path:  {string.Join(", ", pushable.stepPath)}");  
        if (hitObstacle) 
        {
            pushable.RemoveLastFromPath();
            return true;
        }
        
        Success(this, pushable, movement);
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
