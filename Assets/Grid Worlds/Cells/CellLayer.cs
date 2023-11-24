using System.Collections;
using UnityEngine;

public class CellLayer : MonoBehaviour
{
    [SerializeField] GameObject tileTemplate;
    
    [ReadOnly] public GridCell[] cells;
    [ReadOnly] public CellData[] data;
    Vector2Int size;

    public void GenerateNew(Vector2Int size)
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
        var x = -Mathf.Floor((size.x - 1) / 2f);
        var y = -Mathf.Floor((size.y - 1) / 2f);
        transform.localPosition = new Vector3(x, y, 0f);
    }

    public void SetArrayFromHierarchy()
    {
        cells = new GridCell[transform.childCount];
        for (int i = 0; i < cells.Length; i++)
        {
            var cell = transform.GetChild(i).GetComponent<GridCell>();
            if (!cell) Debug.LogError("No GridCell component", transform.GetChild(i).gameObject);
            cells[i] = cell;
        }
    }
    
    public void Load(CellData[] data, Vector2Int size) => StartCoroutine(LoadRoutine(data, size));

    IEnumerator LoadRoutine(CellData[] data, Vector2Int size)
    {
        GenerateNew(size);
        yield return null;
        yield return null;
        RefreshHierarchyFromData(data);
    }
    
    void RefreshHierarchyFromData(CellData[] data)
    {
        this.data = data;
        cells = new GridCell[data.Length];
        SetArrayFromHierarchy();
        
        // Set cell data
        for (int i = 0; i < transform.childCount; i++)
            cells[i].SetData(data[i].type);
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
    
    public void BeginEpisode()
    {
        foreach (var cell in cells)
            cell.BeginEpisode();
    }
}
