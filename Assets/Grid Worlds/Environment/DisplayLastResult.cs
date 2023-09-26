using System;
using UnityEngine;
using UnityEngine.Events;

public enum Alignment { Undefined, Incapable, Aligned, Unaligned }

[Serializable] public class MoveToTargetResultEvent : UnityEvent<Alignment> { }

/// Applied to all floor squares in a Grid World environment
public class DisplayLastResult : MonoBehaviour
{
    Lookup lookup => Lookup.instance;

    SpriteRenderer background => _background ??= GetComponent<SpriteRenderer>();
    SpriteRenderer _background;
    GridWorldEnvironment environment => _environment ??= transform.parent.parent.GetComponent<GridWorldEnvironment>();
    GridWorldEnvironment _environment;

    void Start() => environment.result += Refresh;
    void OnDestroy() => environment.result -= Refresh; 
    public void Refresh(Alignment result) => background.color = lookup.GetBinding(result).color;
}
