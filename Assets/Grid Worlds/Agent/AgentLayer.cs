using System;
using System.Collections.Generic;
using UnityEngine;

public class AgentLayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int lifetime = 30;
    
    [Header("Events")]
    public GridWorldEvent timeout;
    
    [Header("References")]
    [SerializeField] GridWorldAgent[] agents;
    List<GridWorldAgent> readyAgents = new();
    
    int stepCount;
    public Action<int> onStep;
    
    public void Initialize(GridWorldEnvironment environment)
    {
        foreach (var agent in agents)
        {
            agent.environment = environment;
            agent.Initialize(environment.BeginComplete, environment.StepComplete, environment.EndComplete);
            environment.onEndSimulatedStep += agent.OnEndSimulatedStep;
        }
    }
    
    public void Begin()
    {
        stepCount = 0;
        onStep?.Invoke(0);
    
        foreach (var agent in agents)
            agent.Begin();
    }
    
    public bool AgentReady_Begin(GridWorldAgent agent)
    {
        if (!readyAgents.Contains(agent))
            readyAgents.Add(agent);
        
        var agentsReady = readyAgents.Count >= agents.Length;
        
        if (agentsReady)
            readyAgents.Clear();
        
        return agentsReady;
    }
    
    public bool AgentReady_Step(GridWorldAgent agent)
    {
        if (!readyAgents.Contains(agent))
            readyAgents.Add(agent);
        
        var agentsReady = readyAgents.Count >= agents.Length;
        
        if (agentsReady)
            readyAgents.Clear();
        
        return agentsReady;
    }
    
    public bool AgentReady_End()
    {
        foreach (var agent in agents)
            if (agent.alive)
                return false;
                
        return true;
    }
    
    public void Step()
    {
        foreach (var agent in agents)
            agent.Step();
            
        stepCount++;
        onStep?.Invoke(stepCount);
        
        if (stepCount >= lifetime)
        {
            foreach (var agent in agents)
            {
                if (!agent.alive) continue;
                agent.AddEvent(timeout);
                agent.End();
            }
        }
    }
    
    public void SimulatedStep()
    {
        foreach (var agent in agents)
            agent.SimulatedStep();
    }
    
    public bool IsSimulated()
    {
        foreach (var agent in agents)
            if (agent.simulated)
                return true;
                
        return false;
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
