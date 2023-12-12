using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Fire")]
public class LavaCell : GridCellInfo
{
    [SerializeField] GridWorldEvent eventId;

    public override bool Touch(MovingEntity entity, GridCell cell)
    {
        entity.AddEvent(eventId);
        entity.Die();
        
        var gridObject = entity.transform.GetComponent<GridObject>();
        if (gridObject && gridObject.info.destructible)
            gridObject.gameObject.SetActive(false);
            
        return false;
    }
}