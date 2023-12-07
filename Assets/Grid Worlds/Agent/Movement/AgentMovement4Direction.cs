using System.Collections.Generic;
using UnityEngine;

public class AgentMovement4Direction : IAgentMovement
{
    [Tooltip("In heuristic mode, only advance time when the player has pressed a key")]
    [SerializeField] bool heuristicWaitForKeypress;  // * Interface is not serializable, pass in as argument on Awake
    
    int keyHorizontal => Statics.GetAxis("Horizontal");
    int keyVertical => Statics.GetAxis("Vertical");

    int cachedDirection;
    
    const int STAY = 0;
    const int DOWN = 1;
    const int UP = 2;
    const int LEFT = 3;
    const int RIGHT = 4;
    
    DiscretePlacement setPosition;
    List<AgentEffect> actionModifiers;
    
    public void Awake(DiscretePlacement setPosition, List<AgentEffect> actionModifiers) 
    {
        this.setPosition = setPosition;
        this.actionModifiers = actionModifiers; 
    }
    
    public void Update() => UpdatePlayerInputCache();

    void UpdatePlayerInputCache()
    {
        if (cachedDirection != 0) return;
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            cachedDirection = LEFT;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            cachedDirection = RIGHT;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            cachedDirection = DOWN;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            cachedDirection = UP;
    }
    
    public void Move(int[] actions)
    {
        foreach (var modifier in actionModifiers)
            modifier.ModifyActions(ref actions, 5);
    
        switch(actions[0])
        {
            case STAY: break;
            case LEFT: setPosition.MoveLeft(); break;
            case RIGHT: setPosition.MoveRight(); break;
            case DOWN: setPosition.MoveDown(); break;
            case UP: setPosition.MoveUp(); break;
        }
    }

    int[] actions = new int[1];
    public int[] PlayerControl()
    {
        if (cachedDirection == 0)
        {
            if (keyHorizontal == -1)
                actions[0] = LEFT;
            else if (keyHorizontal == 1)
                actions[0] = RIGHT;
            else if (keyVertical == -1)
                actions[0] = DOWN;
            else if (keyVertical == 1)
                actions[0] = UP;
            else
                actions[0] = STAY;
        }
        else
            actions[0] = cachedDirection;
        
        ClearCache();
        return actions;
    }
    
    public void ClearCache() => cachedDirection = 0;
    public bool MoveKeyPressed() => heuristicWaitForKeypress && cachedDirection == 0;
    
    public int[] ActionSpace() => new[] { 4 };
}
