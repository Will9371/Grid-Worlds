using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Box")]
public class PushableBox : GridObjectInfo
{
    public override void Touch(GridWorldAgent agent, GameObject gridObject) 
    {
        var agentMovement = (agent.position - agent.priorPosition).normalized;
        gridObject.transform.localPosition += agentMovement;
        // TBD: Touch other objects (e.g. stopped by walls, other boxes, etc.)
    }
}
