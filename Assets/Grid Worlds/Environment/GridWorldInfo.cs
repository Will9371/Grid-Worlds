using System;
using UnityEngine;

// Error: file clears on starting up Unity (can fix with Save, but this only works for already set-up environment)
// Research: properties vs. fields (should save fields: https://forum.unity.com/threads/data-disappearing-on-scriptableobject.351183/)
[CreateAssetMenu(menuName = "Grid Worlds/Environment")]
public class GridWorldInfo : ScriptableObject
{
    //[HideInInspector] 
    [SerializeField]
    public Vector2 size;
    
    //[HideInInspector] 
    [SerializeField]
    public CellData[] cellData;
    
    //[HideInInspector]
    [SerializeField]
    public ObjectLayerData objectData;

    public void Save(GridWorldEnvironment source)
    {
        Debug.Log($"GridWorldInfo.Save({name})", this);

        size = source.size;
        
        var cellContainer = source.cellLayer.transform;
        cellData = new CellData[cellContainer.childCount];
        for (int i = 0; i < cellData.Length; i++)
            cellData[i] = new CellData(cellContainer.GetChild(i));
            
        objectData = new ObjectLayerData(source.objectLayer);
        
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        #endif
    }
}

[Serializable]
public struct CellData
{
    public GridCellType type;
    
    public CellData(Transform transform)
    {
        var cell = transform.GetComponent<GridCell>();
        type = cell.cellType;
    }
}

