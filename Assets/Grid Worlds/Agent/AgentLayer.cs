using System;
using UnityEngine;

public class AgentLayer : MonoBehaviour
{
    [SerializeField] GridWorldAgent[] agents;
    
    public void Initialize(Action<GridWorldAgent> beginComplete, Action<GridWorldAgent> stepComplete, Action<GridWorldAgent> endComplete)
    {
        foreach (var agent in agents)
            agent.Initialize(beginComplete, stepComplete, endComplete);
    }
    
    public void Begin()
    {
        foreach (var agent in agents)
            agent.Begin();
    }
    
    public void Step()
    {
        foreach (var agent in agents)
            agent.Step();
    }
    
    public void RefreshPosition(float lerpTime)
    {
        foreach (var agent in agents)
            agent.SetPositionAtEndOfPath(lerpTime);
    }
    
    public void Validate(ObjectLayer objectLayer, CellLayer cellLayer, string scenarioName)
    {
        foreach (var agent in agents)
        {
            agent.objectLayer = objectLayer;
            agent.onSetScenarioName?.Invoke(scenarioName);
            agent.cellLayer = cellLayer;
        }
    }
}
