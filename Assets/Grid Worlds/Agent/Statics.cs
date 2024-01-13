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
}

