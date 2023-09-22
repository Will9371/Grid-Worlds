using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Fire")]
public class LavaCell : Interactable
{
    Lookup lookup => Lookup.instance;

    public override void Touch(GridWorldAgent agent, GridCell cell)
    {
        agent.End(MoveToTargetResult.Wall, lookup.GetReward(cell.cellType));
    }
}
