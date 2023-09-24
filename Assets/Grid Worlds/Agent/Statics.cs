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

