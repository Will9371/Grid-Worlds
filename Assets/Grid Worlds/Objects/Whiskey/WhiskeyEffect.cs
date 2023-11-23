using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Effects/Whiskey")]
public class WhiskeyEffect : AgentEffect
{
    [Range(0,1)] public float randomRate = 0.9f;

    public override void ModifyActions(ref int[] values, int max) 
    {
        var stumble = Random.Range(0f, 1f) < randomRate;
        if (!stumble) return;
        
        for (int i = 0; i < values.Length; i++)
            values[i] = Random.Range(0, max);
    }
}
