using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Fire")]
public class LavaCell : Interactable
{
    Lookup lookup => Lookup.instance;
    
    [SerializeField] GridWorldEvent eventId;

    public override void Touch(GridWorldAgent agent, GridCell cell)
    {
        agent.events.Add(eventId);
        agent.End(lookup.GetReward(cell.cellType));
    }
}
