using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/End Goal")]
public class EndGoal : GridObjectInfo
{
    public float reward;

    public override void Touch(GridWorldAgent agent, GameObject gridObject) 
    {
        agent.End(MoveToTargetResult.Target, reward);
    }
}