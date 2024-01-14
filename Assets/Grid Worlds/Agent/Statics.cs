// RENAME

using System;
using System.Collections;
using UnityEngine;

public static class Statics
{
    public const int spatialDimensions = 2;

    public static int GetAction(string axis)
    {
        var value = GetAxis(axis);
        switch (value)
        {
            case -1: return 1;
            case 1: return 2;
            default: return 0;
        }
    }
    
    public static int GetAxis(string axis) => TrinaryInt(Input.GetAxisRaw(axis));
    
    public static int TrinaryInt(float value, float sensitivity = 0f)
    {
        if (value < 0f - sensitivity) return -1;
        if (value > 0f + sensitivity) return 1;
        return 0;
    }
    
    public static IEnumerator DelayFunction(Action function, int delay = 1)
    {
        for (int i = 0; i < delay; i++)
            yield return null;
            
        function?.Invoke();
    }
    
    public static string PositionString(Transform transform) => PositionString(transform.localPosition.x, transform.localPosition.y);
    public static string PositionString(float x, float y) => $"({x}, {y})";
    
    public static int[] ActionNamesToIds(string[] names)
    {
        int[] ids = new int[names.Length];
        
        for (int i = 0; i < ids.Length; i++)
            ids[i] = ActionNameToId(names[i]);
            
        return ids;
    }
    
    public static int ActionNameToId(string name)
    {
        switch (name)
        {
            case "Move Stay": return 0;
            case "Move Down": return 1;
            case "Move Up": return 2;
            case "Move Left": return 3;
            case "Move Right": return 4;
            default: return -1;
        }
    }
    
    public static string[] ActionIdsToNames(int[] ids)
    {
        string[] names = new string[ids.Length];
        
        for (int i = 0; i < names.Length; i++)
            names[i] = ActionIdToName(ids[i]);
            
        return names;
    }
    
    public static string ActionIdToName(int id)
    {
        switch (id)
        {
            case 0: return "Move Stay";
            case 1: return "Move Down";
            case 2: return "Move Up";
            case 3: return "Move Left";
            case 4: return "Move Right";
            default: return "Move Unknown";
        }
    }
}

