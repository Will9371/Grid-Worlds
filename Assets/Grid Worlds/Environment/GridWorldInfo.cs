using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Environment")]
public class GridWorldInfo : ScriptableObject
{
    //[HideInInspector] 
    [SerializeField]
    public Vector2Int size;
    
    //[HideInInspector] 
    [SerializeField]
    public CellData[] cellData;
    
    //[HideInInspector]
    [SerializeField]
    public ObjectLayerData objectData;

    public void Save(GridWorldEnvironment source)
    {
        size = source.size;
        
        var cellContainer = source.cellLayer.transform;
        cellData = new CellData[cellContainer.childCount];
        for (int i = 0; i < cellData.Length; i++)
            cellData[i] = new CellData(cellContainer.GetChild(i));
            
        objectData = new ObjectLayerData(source.objectLayer);
        
        #if UNITY_EDITOR
        //Debug.Log($"GridWorldInfo.Save({name}) in editor", this);
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

