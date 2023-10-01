using System;
using System.Collections;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class ObjectLayer : MonoBehaviour
{
    #region Save System [WIP, Future Use]
    
    //[HideInInspector] 
    public ObjectLayerData data;
    [HideInInspector] public bool refreshData = false;
    
    void OnValidate()
    {
        if (data.values.Length != elements.Length || refreshData)
        {
            refreshData = false;
            data = new ObjectLayerData(this);
        }
    }
    
    #endregion

    public GridObject[] elements
    {
        get
        {
            if (_elements == null)
                _elements = GetComponentsInChildren<GridObject>();
                
            return _elements;
        }
    }
    GridObject[] _elements;
    
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
    
    public void Load()
    {
        // Generate objects
        DestroyObjects();
        for (int i = 0; i < elements.Length; i++)
            StartCoroutine(GenerateObject(data.values[i], transform));
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

    void DestroyObjects()
    {        
        for (int i = elements.Length - 1; i >= 0; i--)
            StartCoroutine(DestroyObject(elements[i].gameObject));
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
            values[i] = source.elements[i].data;
    }
}
