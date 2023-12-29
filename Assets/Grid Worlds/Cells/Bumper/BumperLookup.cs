using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Bumper Lookup")]
public class BumperLookup : ScriptableObject
{
    [SerializeField] Binding[] bindings;

    public Binding GetBinding(Bool4 directions)
    {
        foreach (var binding in bindings)
            if (directions.Compare(binding.directions))
                return binding;
                
        //Debug.LogError($"Invalid setting {directions.left} {directions.right} {directions.up} {directions.down}");        
        return null;
    }
    
    [Serializable]
    public class Binding
    {
        public Bool4 directions;
        public BumperCell info;
    }
}

[Serializable]
public class Bool4
{
    public bool left, right, up, down;
    public bool Compare(Bool4 other) => left == other.left && right == other.right && up == other.up && down == other.down;
}