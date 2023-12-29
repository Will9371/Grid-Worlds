using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// * Change back to GridObject...ugh...unless I can find a more elegant way to set parameters on the instance level...
    // One SO per configuration, properly labelled (Left Right Down, Right Up, Left Down, etc.)
    // Single generic SO field added to GridCell for extra processing...
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
        entity.AddToPathIfOpen(nextPosition, true);
    }
    
    Vector3 GetRandomDirection()
    {
        float roll = Random.Range(0f, 1f);
        
        if (roll < pushLeftChance) return Vector3.left;
        else roll -= pushLeftChance;
        if (roll < pushRightChance) return Vector3.right;
        else roll -= pushRightChance;
        if (roll < pushUpChance) return Vector3.up;
        else roll -= pushUpChance;
        if (roll < pushDownChance) return Vector3.down;
        else roll -= pushDownChance;
        
        return Vector3.zero;
    }
}
