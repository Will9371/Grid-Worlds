using UnityEngine;

public class ValidateBumper : MonoBehaviour
{
    [SerializeField] GridCell cell;
    [SerializeField] BumperLookup lookup;
    public Bool4 directions;
    
    public void OnValidate()
    {
        if (cell.cellType != GridCellType.Bumper) return;
        var binding = lookup.GetBinding(directions);
        if (binding == null) return;
        cell.instanceInfo = binding.info;
        cell.instanceInfo.LateValidate(cell);
    }
}
