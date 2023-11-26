using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Box")]
public class PushableBox : GridObjectInfo
{
    public override void Touch(MovingEntity source, GameObject self) 
    {
        var pushable = self.GetComponent<MovingEntityMono>();
        if (!pushable) return;
        
        pushable.process.SetPriorPosition();
        
        var movement = Vector3.zero;
        if (!source.lightweight) 
        {
            movement = (source.position - source.priorPosition).normalized;
            pushable.process.position += movement;
        }
        
        pushable.process.CheckForColliders();
        source.ReturnToPriorPosition();
        
        if (pushable.process.priorPosition != pushable.process.position)
            Success(this, pushable.process, movement);
    }
    
    public void Touch(PushableBox info, MovingEntity instance, Vector3 movement) 
    {
        instance.SetPriorPosition();
        instance.position += movement;
        instance.CheckForColliders();
        
        if (instance.priorPosition != instance.position)
            Success(info, instance, movement);
    }
    
    protected virtual void Success(PushableBox info, MovingEntity instance, Vector3 movement) { }
}
