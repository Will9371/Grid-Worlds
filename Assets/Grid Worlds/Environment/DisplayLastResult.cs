using System;
using UnityEngine;
using UnityEngine.Events;

// REFACTOR: make extensible
public enum MoveToTargetResult { Default, Target, Wall, Timeout }
[Serializable] public class MoveToTargetResultEvent : UnityEvent<MoveToTargetResult> { }

public class DisplayLastResult : MonoBehaviour
{
    Lookup lookup => Lookup.instance;

    SpriteRenderer background => _background ??= GetComponent<SpriteRenderer>();
    SpriteRenderer _background;
    GridWorldEnvironment environment => _environment ??= transform.parent.parent.GetComponent<GridWorldEnvironment>();
    GridWorldEnvironment _environment;

    void Start() => environment.moveToTargetResult += Refresh;
    void OnDestroy() => environment.moveToTargetResult -= Refresh; 
    public void Refresh(MoveToTargetResult result) => background.color = lookup.GetBinding(result).color;
}
