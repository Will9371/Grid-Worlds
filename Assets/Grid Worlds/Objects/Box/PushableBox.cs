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
        
        // Lighter objects (e.g. ball) cannot push heavier objects (e.g. box)
        //if (source.lightweight) return true;
        
        var movement = source.moveDirection;
        nextPosition += movement;
        pushable.AddToPath(nextPosition);
        
        //Debug.Log($"{movement} {nextPosition} {pushable.stepPath.Count}");
        //Debug.Log($"Full path:  {string.Join(", ", pushable.stepPath)}"); 
        
        var isBlocked = pushable.CheckForColliders(nextPosition);
        if (isBlocked) return true;
        
        Success(this, pushable, movement);
        return true;
    }

    protected virtual void Success(PushableBox info, MovingEntity instance, Vector3 movement) { }
}
