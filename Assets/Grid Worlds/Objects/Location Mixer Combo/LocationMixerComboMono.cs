using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LocationMixerComboMono : MonoBehaviour
{
    [SerializeField] GameObject[] objs;
    [SerializeField] Combo[] validPositions;

    public void BeginEpisode()
    {
        var index = Random.Range(0, validPositions.Length);
        var combo = validPositions[index];

        for (int i = 0; i < objs.Length; i++)
            objs[i].transform.localPosition = combo.positions[i];
    }
    
    [SerializeField] bool validate;
    void OnValidate()
    {
        if (!validate) return;
        validate = false;
        BeginEpisode();
    }
    
    [Serializable] public struct Combo { public Vector3[] positions; }
}
