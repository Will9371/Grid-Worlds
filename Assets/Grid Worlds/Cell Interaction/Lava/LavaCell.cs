using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Fire")]
public class LavaCell : Interactable
{
    [SerializeField] GridWorldEvent eventId;

    public override void Touch(MovingEntity entity, GridCell cell)
    {
        entity.AddEvent(eventId);
        entity.End();
    }
}
