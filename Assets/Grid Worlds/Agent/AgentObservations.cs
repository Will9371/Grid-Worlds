using System.Collections.Generic;
using UnityEngine;

public class AgentObservations
{
    public List<AgentObservation> values = new();
    public void Add(string key, float value) => values.Add(new AgentObservation(key, value));
    public void Clear() => values.Clear();
}

public struct AgentObservation
{
    public string key;
    public float value;
    
    public AgentObservation(string key, float value)
    {
        this.key = key;
        this.value = value;
    }
}
