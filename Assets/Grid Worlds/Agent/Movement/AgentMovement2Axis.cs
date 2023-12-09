using System.Collections.Generic;
using UnityEngine;

public class AgentMovement2Axis : IAgentMovement
{
    [Tooltip("In heuristic mode, only advance time when the player has pressed a key")]     // Inactive, future use
    bool heuristicWaitForKeypress;
    
    int keyHorizontal => Statics.GetAction("Horizontal");
    int keyVertical => Statics.GetAction("Vertical");

    int cachedHorizontal;
    int cachedVertical;
    
    const int STAY = 0;
    const int DOWN = 1;
    const int UP = 2;
    const int LEFT = 1;
    const int RIGHT = 2;
    
    DiscretePlacement setPosition;
    List<AgentEffect> actionModifiers;
    
    public void Awake(DiscretePlacement setPosition, List<AgentEffect> actionModifiers) 
    {
        this.setPosition = setPosition;
        this.actionModifiers = actionModifiers; 
    }
    
    public void Update()
    {
        UpdatePlayerInputCache();
    }

    void UpdatePlayerInputCache()
    {
        if (cachedHorizontal == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                cachedHorizontal = LEFT;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                cachedHorizontal = RIGHT;
        }
        else if (cachedVertical == 0)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                cachedVertical = DOWN;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                cachedVertical = UP;
        } 
    }
    
    public Vector3 Move(int[] actions)
    {
        foreach (var modifier in actionModifiers)
            modifier.ModifyActions(ref actions, 3);
    
        var result = setPosition.Zero();
        switch(actions[0])
        {
            case STAY: break;
            case LEFT: result += Vector3.left; break;
            case RIGHT: result += Vector3.right; break;
        }
        switch(actions[1])
        {
            case STAY: break;
            case DOWN: result += Vector3.down; break;
            case UP: result += Vector3.up; break;
        }
        return result;
    }

    int[] actions = new int[2];
    public int[] PlayerControl()
    {
        var horizontal = cachedHorizontal == 0 ? keyHorizontal : cachedHorizontal;
        var vertical = cachedVertical == 0 ? keyVertical : cachedVertical;
        
        actions[0] = horizontal;
        actions[1] = vertical;
        ClearCache();

        return actions;
    }
    
    public void ClearCache()
    {
        cachedHorizontal = 0;
        cachedVertical = 0;        
    }
    
    public bool MoveKeyPressed() => heuristicWaitForKeypress && cachedHorizontal == 0 && cachedVertical == 0;
    
    public int[] ActionSpace() => new[] { 2, 2 };
}
