using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Box")]
public class PushableBox : GridObjectInfo
{
    public override void Touch(MovingEntity entity, GameObject gridObject) 
    {
        var box = gridObject.GetComponent<MovingEntityMono>();
        if (!box) return;
        
        box.process.SetPriorPosition();
        var movement = (entity.position - entity.priorPosition).normalized;
        gridObject.transform.localPosition += movement;
        
        box.process.CheckForColliders();
        
        if (box.process.priorPosition == box.process.position)
            entity.ReturnToPriorPosition();
    }
}
