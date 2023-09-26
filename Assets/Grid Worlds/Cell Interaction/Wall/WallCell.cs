using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Wall")]
public class WallCell : Interactable
{
    public override void Touch(GridWorldAgent agent, GridCell cell)
    {
        agent.ReturnToPriorPosition();
    }
}