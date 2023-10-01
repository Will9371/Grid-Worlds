using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellLayer : MonoBehaviour
{
    [SerializeField] GameObject tileTemplate;

    //[HideInInspector] 
    public GridCell[] cells;
    Vector2 size;
    
    public void BeginRefresh() => StartCoroutine(DelayRefresh());
    IEnumerator DelayRefresh()
    {
        yield return null;
        yield return null;
        RefreshCells();
    }
    
    public void GenerateNew(Vector2 size)
    {
        this.size = size;
        DestroyCells();
        
        for (int y = 0; y < size.y; y++)
            for (int x = 0; x < size.x; x++)
                StartCoroutine(GenerateCell(tileTemplate, transform, x, y));
            
        Recenter();
    }
    
    void Recenter()
    {
        var x = -(size.x - 1) / 2f;
        var y = -(size.y - 1) / 2f;
        transform.localPosition = new Vector3(x, y, 0f);
    }

    public void Refresh(GridWorldInfo layout)
    {
        // Set cells
        if (cells == null || transform.childCount != layout.cellData.Length)
            RefreshCells();
        
        // Set cell data
        for (int i = 0; i < transform.childCount; i++)
            cells[i].SetCellData(layout.cellData[i].type);
    }

    public void RefreshCells()
    {
        cells = new GridCell[transform.childCount];
        for (int i = 0; i < cells.Length; i++)
        {
            var cell = transform.GetChild(i).GetComponent<GridCell>();
            if (!cell) Debug.LogError("No GridCell component", transform.GetChild(i).gameObject);
            cells[i] = cell;
        }
    }
    
    public void Load(GridWorldInfo layout)
    {
        // Generate cells
        GenerateNew(layout.size);
        BeginRefresh();
    }
    
    IEnumerator GenerateCell(GameObject prefab, Transform container, int x, int y)
    {
        yield return null;
        var newObject = Instantiate(prefab, container);
        newObject.transform.localPosition = new Vector3(x, y, 0f);
    }
    
    void DestroyCells()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            StartCoroutine(DestroyObject(transform.GetChild(i).gameObject));
    }
    
    IEnumerator DestroyObject(GameObject value)
    {
        yield return null;
        DestroyImmediate(value);
    }
}
