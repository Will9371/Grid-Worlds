using System;
using System.Collections.Generic;
using UnityEngine;

public class GridWorldEnvironment : MonoBehaviour
{
    [Header("References")]
    public ObjectLayer objectLayer;
    public CellLayer cellLayer;
    
    [Header("Settings")]
    [SerializeField] GridWorldInfo layout;
    [SerializeField] GridWorldObjective objective;
    public Vector2 size;
    
    [Header("Editor Commands")]
    [Tooltip("Click this when changing the size of the grid (or to clear the cells)")]
    [SerializeField] bool generateNew;
    [Tooltip("Click this if the cells array is off")]
    [SerializeField] bool refreshCellData;
    [Tooltip("Click this if the object array is off")]
    [SerializeField] bool refreshObjectData;
    
    [Tooltip("Copy data from environment to the layout ScriptableObject")]
    [SerializeField] bool save;
    [Tooltip("Copy data from the layout ScriptableObject to the environment")]
    //[HideInInspector] // WIP: reactivate when working
    [SerializeField] bool load;
    
    /// Mark scene as dirty on refresh, so that ScriptableObject gets saved
    [HideInInspector] public bool toggle;

    public Action<Alignment> result;
    
    public GridCell[] cells => cellLayer.cells;
    
    public void EndEpisode(List<GridWorldEvent> events)
    {
        if (!objective) return;
        var resultValue = objective.GetResult(events);
        result?.Invoke(resultValue);
    }

    void OnValidate()
    {
        size = new Vector2(Mathf.Floor(size.x), Mathf.Floor(size.y));
        
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
}