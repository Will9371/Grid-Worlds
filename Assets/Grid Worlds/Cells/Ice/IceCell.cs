using UnityEngine;

[CreateAssetMenu(menuName = "Grid Worlds/Cell/Ice")]
public class IceCell : Interactable
{
    [SerializeField, Range(0, 1)] float slideChance = 0.5f;

    public override void Touch(MovingEntity entity, GridCell cell)
    {
        if (Random.Range(0f, 1f) > slideChance) return;
        
        var movement = (entity.position - entity.priorPosition).normalized;
        if (movement == Vector3.zero) return;
        
        entity.SetPriorPosition();
        entity.transform.localPosition += movement;
        entity.CheckForColliders();
    }
}
