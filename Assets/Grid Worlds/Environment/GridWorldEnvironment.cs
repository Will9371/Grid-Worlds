using System;
using System.Collections.Generic;
using UnityEngine;

public class GridWorldEnvironment : MonoBehaviour
{
    [Header("References")]
    public AgentLayer agentLayer;
    public ObjectLayer objectLayer;
    public CellLayer cellLayer;
    
    [Header("Settings")]
    [SerializeField] string scenarioName;
    [SerializeField] GridWorldObjective objective;
    [VectorLabels("Width", "Height")]
    public Vector2Int size;
    [Tooltip("Delay between each timestep. Set to 0 for max speed.")]
    [SerializeField] float realStepDelay = .25f;
    [Tooltip("Forces moving objects to pause between steps. Cannot be greater than stepDelay.")]
    [SerializeField] float realStepDelayBuffer = .05f;
    [Tooltip("Delay between each timestep. Set to 0 for max speed.")]
    [SerializeField] float simulatedStepDelay = .15f;
    [Tooltip("Forces moving objects to pause between steps. Cannot be greater than stepDelay.")]
    [SerializeField] float simulatedStepDelayBuffer = .05f;
    [Tooltip("Delay before episode begins")]
    [SerializeField] float beginDelay = .5f;
    [Tooltip("Delay before episode ends")]
    [SerializeField] float endDelay = 0.5f;
    
    [Header("Editor Commands")]
    [Tooltip("Click this when changing the size of the grid (or to clear the cells)")]
    [SerializeField] bool generateNew;
    [Tooltip("Save file")]
    [SerializeField] GridWorldInfo layout;
    [Tooltip("Copy data from environment to the layout ScriptableObject")]
    [SerializeField] bool save;
    [Tooltip("Copy data from the layout ScriptableObject to the environment")]
    [SerializeField] bool load;
    
    [Header("Debug")]
    [Tooltip("Click this to prompt the editor to complete a refresh action")]
    [SerializeField] bool tick;
    [Tooltip("Click this if the cells array is off")]
    [SerializeField] bool refreshCellData;
    [Tooltip("Click this if the object array is off")]
    [SerializeField] bool refreshObjectData;
    
    /// Mark scene as dirty on refresh, so that ScriptableObject gets saved
    [HideInInspector] public bool toggle;
    
    public bool simulated;
    public void RefreshSimulated()
    {
        var anyAgentSimulated = agentLayer.IsSimulated();
        
        if (simulated != anyAgentSimulated)
            onSetSimulated?.Invoke(anyAgentSimulated);
            
        simulated = anyAgentSimulated;
    }
    public Action<bool> onSetSimulated;
    public Action onEndSimulatedStep;

    public Action<Alignment> result;
    
    void Start()
    {
        agentLayer.Initialize(this);
        cellLayer.Initialize(this);
        objectLayer.Initialize(this);
        BeginEpisode();
    }
    
    void BeginEpisode() 
    {
        //Debug.Log("Environment.BeginEpisode()"); 
        cellLayer.BeginEpisode();
        objectLayer.BeginEpisode();
        agentLayer.Begin();
    }
    
    public void BeginComplete(GridWorldAgent agent)
    {
        if (!agentLayer.AgentReady_Begin(agent)) return;
        Invoke(nameof(Step), beginDelay);
    }
    
    void Step() => agentLayer.Step();
    
    void SimulatedStep() 
    {
        onEndSimulatedStep?.Invoke();
        agentLayer.SimulatedStep();
    }
    
    public void StepComplete(GridWorldAgent agent)
    {
        if (!simulated && !agentLayer.AgentReady_Step(agent)) return;
        if (simulated && !agentLayer.AgentReady_Step(agent)) return;
        
        var stepDelay = simulated ? simulatedStepDelay : realStepDelay;
        var stepDelayBuffer = simulated ? simulatedStepDelayBuffer : realStepDelayBuffer;
        
        var delay = stepDelay - stepDelayBuffer;
        agentLayer.RefreshPosition(delay);
        objectLayer.RefreshPosition(delay);
        
        var nextStep = simulated ? "SimulatedStep" : "Step";
        Invoke(nextStep, stepDelay);
    }
    
    public void EndComplete(GridWorldAgent agent)
    {
        if (!agentLayer.AgentReady_End()) return;
        BroadcastResult(agent.events);
        
        // hack
        CancelInvoke(nameof(Step));
        CancelInvoke(nameof(SimulatedStep));
        
        Invoke(nameof(BeginEpisode), endDelay);
    }
    
    void BroadcastResult(List<GridWorldEvent> events)
    {
        if (!objective) return;
        var resultValue = objective.GetResult(events);
        result?.Invoke(resultValue);
    }
    
    void OnValidate()
    {
        if (tick) tick = false;
        
        if (beginDelay < 0) beginDelay = 0f;
        if (endDelay < 0) endDelay = 0f;
        if (realStepDelay < 0) realStepDelay = 0f;
        if (realStepDelayBuffer < 0) realStepDelayBuffer = 0f;
        if (simulatedStepDelay < 0) simulatedStepDelay = 0f;
        if (simulatedStepDelayBuffer < 0) simulatedStepDelayBuffer = 0f;
        if (realStepDelayBuffer > realStepDelay) realStepDelayBuffer = realStepDelay;
        if (simulatedStepDelayBuffer > simulatedStepDelay) simulatedStepDelayBuffer = simulatedStepDelay;
            
        agentLayer.Validate(objectLayer, cellLayer, scenarioName);
        
        if (save)
        {
            save = false;
            if (layout) layout.Save(this);
            else Debug.LogError("Cannot save, info field null");
        }
        if (load)
        {
            load = false;
            if (layout) Load();
            else Debug.LogError("Cannot load, info field null");
        }
        
        if (generateNew)
        {
            generateNew = false;
            cellLayer.GenerateNew(size);
        }
        if (refreshCellData)
        {
            refreshCellData = false;
            cellLayer.SetArrayFromHierarchy();
        }
        if (refreshObjectData)
        {
            refreshObjectData = false;
            objectLayer.SetArrayFromHierarchy();
        }
        
        toggle = !toggle;
    }
    
    void Load()
    {
        Debug.Log($"GridWorldEnvironment.Load{layout.name}", layout);
        cellLayer.Load(layout.cellData, layout.size);
        objectLayer.Load(layout.objectData);
    }
    
    public void SetArraysFromHierarchy()
    {
        cellLayer.SetArrayFromHierarchy();
        objectLayer.SetArrayFromHierarchy();
    }
    
    [SerializeField] SimulatedAgentGenerator simulatedAgentGenerator;
    public void GenerateSimulacrum(List<Vector3> path) => simulatedAgentGenerator.GenerateSimulacrum(path, simulatedStepDelay, simulatedStepDelayBuffer);
}