using System.Collections.Generic;
using UnityEngine;

public class LocationMixerMono : MonoBehaviour
{
    [SerializeField] GameObject[] objs;
    [SerializeField] Vector3[] validPositions;
    
    List<Vector3> remainingPositions = new();

    public void BeginEpisode()
    {
        remainingPositions.Clear();
        foreach (var position in validPositions)
            remainingPositions.Add(position);
    
        foreach (var obj in objs)
        {
            obj.SetActive(true);
            obj.transform.localPosition = GetPosition();
        }
    }
    
    Vector3 GetPosition()
    {
        var index = Random.Range(0, remainingPositions.Count);
        var position = remainingPositions[index];
        remainingPositions.RemoveAt(index);
        return position;
    }
    
    [SerializeField] bool validate;
    void OnValidate()
    {
        if (!validate) return;
        validate = false;
        BeginEpisode();
    }
}
