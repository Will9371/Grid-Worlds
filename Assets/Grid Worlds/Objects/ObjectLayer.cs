using System;
using System.Collections;
using UnityEngine;

public class ObjectLayer : MonoBehaviour
{
    public GridObject[] elements
    {
        get
        {
            if (_elements == null)
                SetArrayFromHierarchy();
                
            return _elements;
        }
    }
    [SerializeField] [ReadOnly]
    GridObject[] _elements;
    public void SetArrayFromHierarchy() 
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
    
        _elements = GetComponentsInChildren<GridObject>();
        
        foreach (var element in _elements)
            element.gameObject.SetActive(!element.data.hide);
    }
    
    GridWorldEnvironment environment;
    public void Initialize(GridWorldEnvironment environment)
    {
        this.environment = environment;
    
        foreach (var element in elements)
        {
            element.environment = environment;
            environment.onSetSimulated += element.OnSetSimulated;
        }
    }
    
    public void BeginEpisode()
    {
        //Debug.Log("ObjectLayer.BeginEpisode()");
        foreach (var element in elements)
            element.BeginEpisode();
    }
    
    public void RefreshPosition(float lerpTime)
    {
        foreach (var element in elements)
            element.RefreshPosition(lerpTime);
    }
    
    public int GetObservationCount()
    {
        int result = 0;
        
        foreach (var element in elements)
            result += element.GetObservationCount();
            
        return result;
    }
    
    public void AddObservations(AgentObservations sensor)
    {
        foreach (var element in elements)
            element.AddObservations(sensor);
    }
    
    public void Load(ObjectLayerData data) => StartCoroutine(LoadRoutine(data));

    IEnumerator LoadRoutine(ObjectLayerData data)
    {
        DestroyObjects();
        yield return null;
        yield return null;
        //Debug.Log(data.values[0].touchInfo);

        // Generate objects
        for (int i = 0; i < elements.Length; i++)
            StartCoroutine(GenerateObject(data.values[i], transform));
            
        yield return null;
        yield return null;
        SetArrayFromHierarchy();        
    }
    
    IEnumerator GenerateObject(GridObjectData data, Transform container)
    {
        yield return null;
        Debug.Log($"Prefab = {data.touchInfo.prefab}, info = {data.touchInfo}");
        var newObject = Instantiate(data.touchInfo.prefab, container).GetComponent<GridObject>();
        newObject.Initialize(data);
    }

    void DestroyObjects()
    {        
        for (int i = elements.Length - 1; i >= 0; i--)
        {
            if (!elements[i]) continue;
            environment.onSetSimulated -= elements[i].OnSetSimulated;
            StartCoroutine(DestroyObject(elements[i].gameObject));
        }
    }
    
    IEnumerator DestroyObject(GameObject value)
    {
        yield return null;
        DestroyImmediate(value);
    }
    
    void OnDestroy()
    {
        foreach (var element in elements)
            if (element != null)
                environment.onSetSimulated -= element.OnSetSimulated;
    }
}

[Serializable]
public struct ObjectLayerData
{
    public GridObjectData[] values;
    
    public ObjectLayerData(ObjectLayer source)
    {
        values = new GridObjectData[source.elements.Length];
        for (int i = 0; i < values.Length; i++)
        {
            source.elements[i].OnValidate();
            values[i] = source.elements[i].data;
        }
    }
}
