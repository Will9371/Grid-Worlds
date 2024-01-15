using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AgentObservations
{
    public List<AgentObservation> values = new();
    public void Add(string position, string cellData, string objectData) => values.Add(new AgentObservation(position, cellData, objectData));
    public void Clear() => values.Clear();
    
    // Rewrite to extract data from AgentObservation[], wait until needed for use case (i.e. MLAgent)
    /*public float[] GetValues()
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
    }*/
    
    public AgentObservation[] GetValues(AgentObservations source)
    {
        Dictionary<string, AgentObservation> processedObservations = new Dictionary<string, AgentObservation>();

        foreach (var observation in source.values)
        {
            if (string.IsNullOrEmpty(observation.position))
                Debug.LogError("Error: Observation has empty position.");
            if (string.IsNullOrEmpty(observation.cellData) && string.IsNullOrEmpty(observation.objectData))
                Debug.LogError($"Error: Both cellData and objectData are empty for position {observation.position}.");
        
            if (!processedObservations.ContainsKey(observation.position))
                processedObservations[observation.position] = observation;
            
            else
            {
                if (string.IsNullOrEmpty(processedObservations[observation.position].cellData) && string.IsNullOrEmpty(observation.cellData))
                    Debug.LogError($"Error: Both cellData fields are empty for position: {observation.position}.  Object data is: {observation.objectData}");
                
                processedObservations[observation.position] = new AgentObservation(
                    processedObservations[observation.position],
                    observation
                );
            }
        }

        values = processedObservations.Values.ToList();
        return processedObservations.Values.ToArray();
    }
}

[Serializable]
public struct AgentObservation
{
    public string position;
    public string cellData;
    public string objectData;
    
    public AgentObservation(string position, string cellData, string objectData)
    {
        this.position = position;
        this.cellData = cellData;
        this.objectData = objectData;
    }
    
    public AgentObservation(AgentObservation o1, AgentObservation o2)
    {
        position = o1.position;
        cellData = o1.cellData == "" ? o2.cellData : o1.cellData;
        objectData = o1.objectData == "" ? o2.objectData : o1.objectData;
    }
}
