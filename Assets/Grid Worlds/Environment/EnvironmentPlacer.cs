using UnityEngine;

public class EnvironmentPlacer : MonoBehaviour
{
    public Vector2[] locations;

    public bool validate;
    void OnValidate()
    {
        if (!validate) return;
        validate = false;
        Refresh();
    }
    
    void Refresh()
    {
        for (int i = 0; i < transform.childCount && i < locations.Length; i++)
            transform.GetChild(i).localPosition = locations[i];
    }
}
