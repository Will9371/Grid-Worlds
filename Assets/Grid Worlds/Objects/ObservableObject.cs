// Consider deprecation

public interface IObservableObject
{
    int observationCount { get; }
    void AddObservations(AgentObservations observations);
}
