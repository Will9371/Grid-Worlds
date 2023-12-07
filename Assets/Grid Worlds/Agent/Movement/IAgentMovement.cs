using System.Collections.Generic;

public interface IAgentMovement
{
    void Awake(DiscretePlacement setPosition, List<AgentEffect> actionModifiers) { }
    void Update() { }
    void ClearCache() { }
    int[] PlayerControl() { return null; }
    void Move(int[] actions) { }
    bool MoveKeyPressed () { return false; }
    int[] ActionSpace();
}

public enum AgentMovementType
{
    Axis2Direction8,
    Direction4,
}