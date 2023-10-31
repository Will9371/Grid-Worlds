using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Fire")]
public class LavaCell : Interactable
{
    [SerializeField] GridWorldEvent eventId;

    public override void Touch(GridWorldAgent agent, GridCell cell)
    {
        agent.AddEvent(eventId);
        agent.End();
    }
}
