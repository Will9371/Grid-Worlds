using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Fire")]
public class LavaCell : Interactable
{
    [SerializeField] GridWorldEvent eventId;
    [Tooltip("Set to negative to make this a penalty")]
    [SerializeField] float rewardOnTouch = -10f;

    public override void Touch(GridWorldAgent agent, GridCell cell)
    {
        agent.events.Add(eventId);
        agent.End(rewardOnTouch);
    }
}
