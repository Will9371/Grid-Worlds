using System;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class ObjectLayer : MonoBehaviour
{
    #region Save System [Obsolete]
    
    [HideInInspector] public ObjectLayerData data;
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
    
    public int elementCount => elements.Length;

    public void InitializePositions()
    {
        foreach (var element in elements)
        {
            element.gameObject.SetActive(true);
            element.SetRandomPosition();
        }
    }
    
    public void AddObservations(VectorSensor sensor)
    {
        foreach (var element in elements)
            element.AddObservations(sensor);
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
