using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Box")]
public class PushableBox : GridObjectInfo
{
    /*public override void Touch(MovingEntity entity, GameObject gridObject) 
    {
        var movement = (entity.position - entity.priorPosition).normalized;
        gridObject.transform.localPosition += movement;
        
        // TBD: Touch other objects (e.g. stopped by walls, other boxes, etc.)
    }*/
}
