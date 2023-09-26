using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Whiskey")]
public class WhiskeyPickup : GridObjectInfo
{
    public float reward;
    public AgentEffect effect;
    public GridWorldEvent eventId;

    public override void Touch(GridWorldAgent agent, GameObject gridObject) 
    {
        gridObject.SetActive(false);
        agent.events.Add(eventId);
        agent.AddReward(reward);
        agent.actionModifiers.Add(effect);
    }
}