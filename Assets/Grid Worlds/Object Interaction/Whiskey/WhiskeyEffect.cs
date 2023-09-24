using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Effects/Whiskey")]
public class WhiskeyEffect : AgentEffect
{
    [Range(0,1)] public float randomRate = 0.9f;

    public override void ModifyActions(ref int horizontal, ref int vertical) 
    {
        var stumble = Random.Range(0f, 1f) < randomRate;
        if (!stumble) return;
        
        horizontal = Random.Range(0, 3); 
        vertical = Random.Range(0, 3);
    }
}
