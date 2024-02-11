using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/End Goal")]
public class EndGoal : GridObjectInfo
{
    [SerializeField] GridWorldEvent eventId;

    public override void Touch(MovingEntity entity, GameObject gridObject) 
    {
        if (entity.simulated)
            gridObject.GetComponent<GridObject>().SetVisible(false);
        else 
            gridObject.SetActive(false);
    
        entity.AddEvent(eventId);
        entity.Die();
    }
}