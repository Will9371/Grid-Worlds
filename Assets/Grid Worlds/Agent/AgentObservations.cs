using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AgentObservations
{
    [SerializeField] List<CellObservation> cellObservations = new();
    
    /// Add a cell observation (consider rename)
    public void Add(string position, string cellData, string objectData) => cellObservations.Add(new CellObservation(position, cellData, objectData));
    void ClearCells() => cellObservations.Clear();
    
    public ObservationData GetObservationData(GridWorldAgent agent)
    {
        var agentData = new AgentObservationData(agent);
        
        ClearCells();
        agent.AddCellObservations(this);
        var cellData = ProcessCellObservations();
        
        return new ObservationData(agentData, cellData);
    }
    
    CellObservation[] ProcessCellObservations()
    {
        Dictionary<string, CellObservation> processedObservations = new Dictionary<string, CellObservation>();

        foreach (var observation in cellObservations)
        {
            // * Are these actually errors?  What if object is processed first?
            if (string.IsNullOrEmpty(observation.position))
                Debug.LogError("Error: Observation has empty position.");
            if (string.IsNullOrEmpty(observation.cellData) && string.IsNullOrEmpty(observation.objectData))
                Debug.LogError($"Error: Both cellData and objectData are empty for position {observation.position}.");
        
            if (!processedObservations.ContainsKey(observation.position))
                processedObservations[observation.position] = observation;
            
            else
            {
                // Error spam, uncomment when this becomes relevant
                //if (string.IsNullOrEmpty(processedObservations[observation.position].cellData) && string.IsNullOrEmpty(observation.cellData))
                //    Debug.LogError($"Error: Both cellData fields are empty for position: {observation.position}.  Object data is: {observation.objectData}");
                
                processedObservations[observation.position] = new CellObservation(
                    processedObservations[observation.position],
                    observation
                );
            }
        }

        cellObservations = processedObservations.Values.ToList();
        return processedObservations.Values.ToArray();
    }
}

[Serializable]
public struct ObservationData
{
    public AgentObservationData agent;
    public CellObservation[] cells;
    
    public ObservationData(AgentObservationData agent, CellObservation[] cells)
    {
        this.agent = agent;
        this.cells = cells;
    }
}

[Serializable]
public struct AgentObservationData
{
    public string id;
    public string[] inventory;
    public string[] lastStepResults;
    
    public AgentObservationData(GridWorldAgent agent)
    {
        this = agent.GetSelfDescription();
    }
    
    public AgentObservationData(string id, string[] inventory, string[] lastStepResults)
    {
        this.id = id;
        this.inventory = inventory;
        this.lastStepResults = lastStepResults;
    }
}

[Serializable]
public struct CellObservation
{
    public string position;
    public string cellData;
    public string objectData;
    
    public CellObservation(string position, string cellData, string objectData)
    {
        this.position = position;
        this.cellData = cellData;
        this.objectData = objectData;
    }
    
    public CellObservation(CellObservation o1, CellObservation o2)
    {
        position = o1.position;
        cellData = o1.cellData == "" ? o2.cellData : o1.cellData;
        objectData = o1.objectData == "" ? o2.objectData : o1.objectData;
    }
}


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