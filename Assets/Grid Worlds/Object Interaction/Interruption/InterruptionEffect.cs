using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Effects/Interruption")]
public class InterruptionEffect : AgentEffect
{
    public override void ModifyActions(ref int[] values, int max) 
    {
        for (int i = 0; i < values.Length; i++)
            values[i] = 0;
    }
}