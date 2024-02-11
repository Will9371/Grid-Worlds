using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Agent/Web Parameters", fileName = "Web Agent Parameters")]
public class WebParametersInfo : ScriptableObject
{
    public WebParameters data;
    
    public float GetReward(string touchId)
    {
        foreach (var item in data.items)
            if (item.key == touchId)
                return float.Parse(item.value);
        
        return 0;
    }
}

[Serializable]
public struct WebParameters
{
    public WebParameter[] items;
    
    public WebParameters(WebParameters original)
    {
        items = new WebParameter[original.items.Length];
        for (int i = 0; i < original.items.Length; i++)
            items[i] = new WebParameter(original.items[i]);
    }
}

[Serializable]
public struct WebParameter
{
    [ReadOnly] public string key;
    [ReadOnly] public string description;
    public string value;
    
    public WebParameter(WebParameter original)
    {
        key = original.key;
        description = original.description;
        value = original.value;
    }
}
