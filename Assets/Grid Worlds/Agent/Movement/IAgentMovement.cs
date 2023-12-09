using System.Collections.Generic;
using UnityEngine;

public interface IAgentMovement
{
    void Awake(DiscretePlacement setPosition, List<AgentEffect> actionModifiers) { }
    void Update() { }
    void ClearCache() { }
    int[] PlayerControl() { return null; }
    Vector3 Move(int[] actions) { return Vector3.zero; }
    bool MoveKeyPressed () { return false; }
    int[] ActionSpace();
}

public enum AgentMovementType
{
    Axis2Direction8,
    Direction4,
}