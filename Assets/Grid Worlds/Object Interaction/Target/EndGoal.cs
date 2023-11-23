using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/End Goal")]
public class EndGoal : GridObjectInfo
{
    [SerializeField] GridWorldEvent eventId;

    public override void Touch(MovingEntity entity, GameObject gridObject) 
    {
        base.Touch(entity, gridObject);
        entity.AddEvent?.Invoke(eventId);
        entity.End?.Invoke();
    }
}