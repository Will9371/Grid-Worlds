using System.Collections.Generic;
using UnityEngine;

public class SimulatedAgentGenerator : MonoBehaviour
{
    [SerializeField] GameObject simulacrum;

    public void GenerateSimulacrum(List<Vector3> path, float delay, float delayBuffer)
    {
        var newSimulacrum = Instantiate(simulacrum).GetComponent<SimulatedAgent>();
        newSimulacrum.FollowPath(path, delay, delayBuffer);
    }
}
