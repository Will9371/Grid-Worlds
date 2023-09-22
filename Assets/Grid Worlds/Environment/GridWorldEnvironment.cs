using System;
using System.Collections;
using UnityEngine;

public class GridWorldEnvironment : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject tileTemplate;
    public Transform tileContainer;
    public ObjectLayer objectLayer;
    
    [Header("Settings")]
    [SerializeField] [HideInInspector] GridWorldInfo info;
    public Vector2 size;
    
    [Header("Editor Commands")]
    [SerializeField] bool generateNew;
    [SerializeField] bool refreshCells;
    
    // Obsolete
    bool save;
    bool load;
    
    /// Mark scene as dirty on refresh
    [HideInInspector] public bool toggle;
    //[HideInInspector] 
    public GridCell[] cells;
    
    public Action<MoveToTargetResult> moveToTargetResult;

    void OnValidate()
    {
        size = new Vector2(Mathf.Floor(size.x), Mathf.Floor(size.y));
        
        // Obsolete
        if (save)
        {
            save = false;
            if (info) info.Save(this);
            else Debug.LogError("Cannot save, info field null");
        }
        if (load)
        {
            load = false;
            if (info) Load();
            else Debug.LogError("Cannot load, info field null");
        }
        
        if (generateNew)
        {
            generateNew = false;
            GenerateNew();
            Recenter();
        }
        
        if (refreshCells)
        {
            refreshCells = false;
            RefreshCells();
        }
        
        toggle = !toggle;
    }
    
    void Load()
    {
        size = info.size;
        
        // Generate cells
        GenerateNew();
        BeginRefresh();
        
        // Generate objects
        DestroyObjects();
        for (int i = 0; i < objectLayer.elementCount; i++)
            StartCoroutine(GenerateObject(objectLayer.data.values[i], objectLayer.transform));
    }

    void GenerateNew()
    {
        DestroyCells();
        
        for (int y = 0; y < size.y; y++)
            for (int x = 0; x < size.x; x++)
                StartCoroutine(GenerateCell(tileTemplate, tileContainer, x, y));
    }
    
    public void Refresh()
    {
        // Set cells
        if (cells == null || tileContainer.childCount != info.cellData.Length)
            RefreshCells();
        
        // Set cell data
        for (int i = 0; i < tileContainer.childCount; i++)
            cells[i].SetCellData(info.cellData[i].type);
    }
    
    void RefreshCells()
    {
        cells = new GridCell[tileContainer.childCount];
        for (int i = 0; i < cells.Length; i++)
        {
            var cell = tileContainer.GetChild(i).GetComponent<GridCell>();
            if (!cell) Debug.LogError("No GridCell component", tileContainer.GetChild(i).gameObject);
            cells[i] = cell;
        }
    }

    IEnumerator GenerateObject(GridObjectData data, Transform container)
    {
        yield return null;
        var newObject = Instantiate(data.touchInfo.prefab, container).GetComponent<GridObject>();
        newObject.transform.localPosition = new Vector3(data.position.x, data.position.y, 0f);
        newObject.data = data;
        newObject.positioner.transform = newObject.transform;
        newObject.positioner.xRange = data.xPlaceRange;
        newObject.positioner.yRange = data.yPlaceRange;
    }
    
    IEnumerator GenerateCell(GameObject prefab, Transform container, int x, int y)
    {
        yield return null;
        var newObject = Instantiate(prefab, container);
        newObject.transform.localPosition = new Vector3(x, y, 0f);
    }
    
    void DestroyCells()
    {
        for (int i = tileContainer.childCount - 1; i >= 0; i--)
            StartCoroutine(DestroyObject(tileContainer.GetChild(i).gameObject));
    }
    
    void DestroyObjects()
    {        
        for (int i = objectLayer.elementCount - 1; i >= 0; i--)
            StartCoroutine(DestroyObject(objectLayer.elements[i].gameObject));
    }
    
    IEnumerator DestroyObject(GameObject value)
    {
        yield return null;
        DestroyImmediate(value);
    }
    
    void Recenter()
    {
        var x = -(size.x - 1) / 2f;
        var y = -(size.y - 1) / 2f;
        tileContainer.localPosition = new Vector3(x, y, 0f);
    }
    
    void BeginRefresh() => StartCoroutine(DelayRefresh());
    IEnumerator DelayRefresh()
    {
        yield return null;
        yield return null;
        RefreshCells();
    }
}