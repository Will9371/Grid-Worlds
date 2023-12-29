using System;
using UnityEngine;

public class ValidateBumper : MonoBehaviour
{
    [SerializeField] GridCell cell;
    [SerializeField] BumperLookup lookup;
    [SerializeField] Bool4 directions;
    
    void OnValidate()
    {
        if (cell.cellType != GridCellType.Bumper) return;
        var binding = lookup.GetBinding(directions);
        if (binding == null) return;
        cell.instanceInfo = binding.info;
        cell.instanceInfo.LateValidate(cell);
    }
}
