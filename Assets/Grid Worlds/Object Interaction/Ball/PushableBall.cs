using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Object/Ball")]
public class PushableBall : PushableBox
{
    [SerializeField, Range(0, 1)] float rollChance = 0.5f;

    protected override void Success(MovingEntity source, GameObject self)
    {
        if (Random.Range(0f, 1f) < rollChance)
        {
            var gridObject = self.GetComponent<GridObject>();
            if (gridObject) gridObject.info.Touch(source, self);
        }
    }
}
