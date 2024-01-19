using UnityEngine;

public class DiscretePlacement
{
    Transform transform;

    public DiscretePlacement(Transform transform)
    {
        this.transform = transform;
    }

    public Vector2 IntPosition(Vector2 value) => new (Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));

    int x => Mathf.RoundToInt(transform.localPosition.x);
    int y => Mathf.RoundToInt(transform.localPosition.y);
    public Vector2 position
    {
        get => new (x, y);
        set => transform.localPosition = IntPosition(value);
    }
    
    public Vector3 Left() => position + Vector2.left;
    public Vector3 Right() => position + Vector2.right;
    public Vector3 Up() => position + Vector2.up;
    public Vector3 Down() => position + Vector2.down;
    public Vector3 Zero() => position;
}
    

