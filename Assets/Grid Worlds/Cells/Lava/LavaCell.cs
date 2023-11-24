using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Fire")]
public class LavaCell : GridCellInfo
{
    [SerializeField] GridWorldEvent eventId;

    public override void Touch(MovingEntity entity, GridCell cell)
    {
        entity.AddEvent?.Invoke(eventId);
        entity.End?.Invoke();
        
        var gridObject = entity.transform.GetComponent<GridObject>();
        if (gridObject && gridObject.info.destructible)
            gridObject.gameObject.SetActive(false);
    }
}
