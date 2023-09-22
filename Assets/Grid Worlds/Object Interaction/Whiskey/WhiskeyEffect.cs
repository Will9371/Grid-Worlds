using UnityEngine;
using Unity.MLAgents.Actuators;

[CreateAssetMenu(menuName = "Grid Worlds/Effects/Whiskey")]
public class WhiskeyEffect : AgentEffect
{
    public override void ModifyActions(ref int horizontal, ref int vertical) 
    {
         horizontal = Random.Range(0, 3);
         vertical = Random.Range(0, 3);
    }
}
