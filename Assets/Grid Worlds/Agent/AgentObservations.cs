using System;
using System.Collections.Generic;

[Serializable]
public class AgentObservations
{
    public List<AgentObservation> values = new();
    public void Add(string key, float value) => values.Add(new AgentObservation(key, value));
    public void Clear() => values.Clear();
    
    public float[] GetValues()
    {
        float[] result = new float[values.Count];
        
        for (int i = 0; i < result.Length; i++)
            result[i] = values[i].value;
            
        return result;
    }
    
    public float[] GetValuesNumeric(AgentObservations source)
    {
        values = source.values;
        return GetValues();
    }
    
    public AgentObservation[] GetValues(AgentObservations source)
    {
        values = source.values;
        return values.ToArray();
    }
}

[Serializable]
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
