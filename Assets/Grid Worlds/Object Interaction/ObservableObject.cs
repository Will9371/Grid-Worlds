using UnityEngine;
using Unity.MLAgents.Sensors;

public interface IObservableObject
{
    int observationCount { get; }
    void AddObservations(VectorSensor sensor);
}
