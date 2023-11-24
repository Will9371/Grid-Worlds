using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Box")]
public class PushableBox : GridObjectInfo
{
    public override void Touch(MovingEntity source, GameObject self) 
    {
        var pushable = self.GetComponent<MovingEntityMono>();
        if (!pushable) return;
        
        pushable.process.SetPriorPosition();
        
        if (!source.lightweight) 
        {
            var movement = (source.position - source.priorPosition).normalized;
            self.transform.localPosition += movement;
        }
        
        pushable.process.CheckForColliders();
        source.ReturnToPriorPosition();
        
        if (pushable.process.priorPosition != pushable.process.position)
            Success(source, self);
        
        /*if (pushable.process.priorPosition == pushable.process.position)
        {
            source.ReturnToPriorPosition();
        }
        else
            Success(source, self);
            */
    }
    
    protected virtual void Success(MovingEntity source, GameObject self) { }
}
