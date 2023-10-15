using System;
using UnityEngine;

public class Int2Test : MonoBehaviour
{
    [VectorLabels("First", "Second")]
    public Vector2Int builtInExample;
    
    [VectorLabels("Width", "Height")]
    public Vector2 regularVector;
}
