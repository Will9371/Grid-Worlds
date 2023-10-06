using System.Collections;
using UnityEngine;

public class EnvironmentPlacer : MonoBehaviour
{
    [ReadOnly] public GameObject prefab;
    [ReadOnly] public Vector2 size;
    [ReadOnly] [SerializeField] int count;
    [ReadOnly] public Vector2 buffer;
    
    public void OnValidate()
    {
        size = new Vector2(Mathf.Floor(size.x), Mathf.Floor(size.y));
        buffer = new Vector2(Mathf.Floor(buffer.x), Mathf.Floor(buffer.y));
        count = Mathf.FloorToInt(size.x * size.y);
    }
    
    public void BeginRefresh() => StartCoroutine(Refresh());
    IEnumerator Refresh()
    {
        RefreshInstances();
        yield return null;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).name = $"{prefab.name} {i + 1}";
        RefreshPositions();  
    }
    
    #region Set Instance Count
    
    void RefreshInstances()
    {
        if (count > transform.childCount)
            for (int i = transform.childCount; i < count; i++)
                StartCoroutine(GenerateInstance());
        else if (count < transform.childCount)
            for (int i = transform.childCount - 1; i >= count; i--)
                StartCoroutine(DestroyInstance(transform.GetChild(i).gameObject));
    }
    
    IEnumerator GenerateInstance()
    {
        yield return null;
        Instantiate(prefab, transform);
    }
    
    IEnumerator DestroyInstance(GameObject value)
    {
        yield return null;
        DestroyImmediate(value);
    }
    
    #endregion
    
    #region Set Positions
    
    void RefreshPositions()
    {
        var prefabEnvironment = prefab.GetComponent<GridWorldEnvironment>();
        var halfScale = new Vector2(GetHalfScale(size.x, prefabEnvironment.size.x, buffer.x), 
                                    GetHalfScale(size.y, prefabEnvironment.size.y, buffer.y));
        int i = 0;
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                var environment = transform.GetChild(i).GetComponent<GridWorldEnvironment>();
                transform.GetChild(i).localPosition = 
                    new Vector2(x * (environment.size.x + buffer.x), 
                                y * (environment.size.y + buffer.y)) - halfScale;
                i++;
            }
        }
    }
    
    float GetHalfScale(float count, float size, float buffer)
    {
        // Odd
        if ((int)count % 2 == 1)
            return Mathf.FloorToInt(count/2f) * (size + buffer);
        
        // Even
        return Mathf.CeilToInt((count - 1)/2f * (size + buffer));
    }
    
    #endregion
}
