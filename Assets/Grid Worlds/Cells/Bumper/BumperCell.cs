using UnityEngine;

// * Refactor if this needs any additional forms of variation
[CreateAssetMenu(menuName = "Grid Worlds/Cell/Bumper")]
public class BumperCell : GridCellInfo
{
    [SerializeField, Range(0, 1)] float pushLeftChance, pushRightChance, pushUpChance, pushDownChance, noPushChance;
    [SerializeField] bool validateChance;
    [SerializeField] Sprite sprite;
    
    float totalChance => pushLeftChance + pushRightChance + pushUpChance + pushDownChance + noPushChance;
    
    void OnValidate()
    {
        if (!validateChance) return;
        validateChance = false;
        
        Debug.Log(GetRandomDirection());
        
        var inputError = 1f - totalChance;
        if (inputError == 0f) return;

        var distributedError = -inputError / 5f;
        pushLeftChance -= distributedError;
        pushRightChance -= distributedError;
        pushUpChance -= distributedError;
        pushDownChance -= distributedError;
        noPushChance -= distributedError;
    }
    
    public override void LateValidate(GridCell cell)
    {
        cell.rend.sprite = sprite;   
    }

    public override void Touch(MovingEntity entity, GridCell cell)
    {
        // Only activate when stepping on cell
        if (entity.moveDirection == Vector3.zero) return;
        var moveDirection = GetRandomDirection();
        if (moveDirection == Vector3.zero) return;
        var nextPosition = entity.lastPosition + moveDirection;
        entity.moveDirection = moveDirection;
        entity.AddToPathIfOpen(nextPosition, true);
    }
    
    Vector3 GetRandomDirection()
    {
        float roll = Random.Range(0f, 1f);
        
        if (roll < pushLeftChance) 
            return Vector3.left;
        roll -= pushLeftChance;
        
        if (roll < pushRightChance) 
            return Vector3.right;
        roll -= pushRightChance;
        
        if (roll < pushUpChance) 
            return Vector3.up;
        roll -= pushUpChance;
        
        if (roll < pushDownChance) 
            return Vector3.down;
        roll -= pushDownChance;
        
        return Vector3.zero;
    }
}
