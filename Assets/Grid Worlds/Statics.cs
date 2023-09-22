// RENAME

using System;
using UnityEngine;

public static class Statics
{
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
}

// Moved to Lookup, delete on confirm
/*
public static ResultColor[] bindings;
public static ResultColor defaultBinding;

public static ResultColor GetBinding(MoveToTargetResult result)
{
    foreach (var binding in bindings)
        if (binding.result == result)
            return binding;
    
    return defaultBinding;         
}

public static EnvironmentSettings settings;

public static GridCellSettings GetGridCellSettings(GridCellType id)
{
    foreach (var value in settings.values)
        if (value.id == id)
            return value;
    
    Debug.LogError($"Invalid setting {id}");
    return settings.values[0];
}

public static float GetReward(GridCellType id) => GetGridCellSettings(id).rewardOnTouch;
*/


/*
[Serializable]
public struct EnvironmentSettings
{
    public GridCellSettings[] values;
}
*/



