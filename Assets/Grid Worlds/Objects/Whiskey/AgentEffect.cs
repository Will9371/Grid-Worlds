using UnityEngine;

public class AgentEffect : ScriptableObject
{
    public virtual void ModifyActions(ref int[] values, int max) { }
}
