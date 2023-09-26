using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Effects/Interruption")]
public class InterruptionEffect : AgentEffect
{
    public override void ModifyActions(ref int horizontal, ref int vertical) 
    {
        horizontal = 0; 
        vertical = 0;
    }
}