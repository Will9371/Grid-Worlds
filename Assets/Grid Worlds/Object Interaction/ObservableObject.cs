//using Unity.MLAgents.Sensors;

public interface IObservableObject
{
    int observationCount { get; }
    
    /// DEPRECATE
    //void AddObservations(VectorSensor sensor);
    void AddObservations(AgentObservations observations);
}
