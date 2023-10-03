using System;
using System.Collections;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class ObjectLayer : MonoBehaviour
{
    #region Save System [WIP, Future Use]
    
    //[HideInInspector] 
    //public 
    //[SerializeField]
    //ObjectLayerData data;
    //[HideInInspector] public bool refreshData = false;
    
    /*void OnValidate()
    {
        if (data.values.Length != elements.Length || refreshData)
        {
            refreshData = false;
            data = new ObjectLayerData(this);
        }
    }*/
    
    #endregion

    public GridObject[] elements
    {
        get
        {
            if (_elements == null)
                SetArrayFromHierarchy();
                
            return _elements;
        }
    }
    [SerializeField]
    GridObject[] _elements;
    public void SetArrayFromHierarchy() => _elements = GetComponentsInChildren<GridObject>();
    
    public void InitializePositions()
    {
        foreach (var element in elements)
        {
            element.gameObject.SetActive(true);
            element.SetRandomPosition();
        }
    }
    
    public int GetObservationCount()
    {
        int result = 0;
        
        foreach (var element in elements)
            result += element.GetObservationCount();
            
        return result;
    }

    public void AddObservations(VectorSensor sensor)
    {
        foreach (var element in elements)
            element.AddObservations(sensor);
    }
    
    public void Load(ObjectLayerData data) => StartCoroutine(LoadRoutine(data));

    IEnumerator LoadRoutine(ObjectLayerData data)
    {
        //this.data = data;
        
        DestroyObjects();
        yield return null;
        yield return null;

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
        var newObject = Instantiate(data.touchInfo.prefab, container).GetComponent<GridObject>();
        newObject.Initialize(data);
    }

    void DestroyObjects()
    {        
        for (int i = elements.Length - 1; i >= 0; i--)
        {
            if (!elements[i]) continue;
            StartCoroutine(DestroyObject(elements[i].gameObject));
        }
    }
    
    IEnumerator DestroyObject(GameObject value)
    {
        yield return null;
        DestroyImmediate(value);
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
            source.elements[i].OnValidate(); // SetDataFromHierarchy()
            values[i] = source.elements[i].data;
        }
    }
}
