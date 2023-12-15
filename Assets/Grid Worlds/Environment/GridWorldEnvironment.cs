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
    [SerializeField] float stepDelay = .25f;
    [SerializeField] float beginDelay = .5f;
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

    public Action<Alignment> result;
    
    public GridCell[] cells => cellLayer.cells;
    
    void Start()
    {
        agentLayer.Initialize(BeginComplete, StepComplete, EndComplete);
        BeginEpisode();
    }
    
    void BeginEpisode() 
    {
        //Debug.Log("Environment.BeginEpisode()"); 
        cellLayer.BeginEpisode();
        objectLayer.BeginEpisode();
        agentLayer.Begin();
    }
    
    // TBD: check if all agents complete (on AgentLayer)
    void BeginComplete(GridWorldAgent agent)
    {
        Invoke(nameof(Step), beginDelay);
    }
    
    void Step()
    {
        agentLayer.Step();
    }
    
    // TBD: check if all agents complete (on AgentLayer)
    void StepComplete(GridWorldAgent agent)
    {
        agentLayer.RefreshPosition(stepDelay - 0.05f);      // Expose buffer
        objectLayer.RefreshPosition(stepDelay - 0.05f);
        Invoke(nameof(Step), stepDelay);
    }
    
    // TBD: check if all agents complete (on AgentLayer)
    void EndComplete(GridWorldAgent agent)
    {
        BroadcastResult(agent.events);
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
        
        // Stabilize version and verify necessary before uncommenting or deleting
        //agentLayer.Validate(objectLayer, scenarioName);
        
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
}