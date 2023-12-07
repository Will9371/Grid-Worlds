using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentLayer : MonoBehaviour
{
    [SerializeField] GridWorldAgent[] elements;
    
    public void Begin()
    {
        //Debug.Log("Agents.Begin");
        foreach (var element in elements)
            element.onClearBeginFlag?.Invoke();
    }
    
    public void Step()
    {
        //Debug.Log("Agents.Step");
        foreach (var element in elements)
            element.onClearStepFlag?.Invoke();
    }
    
    public void End()
    {
        //Debug.Log("Agents.End");
        foreach (var element in elements)
            element.onClearEndFlag?.Invoke();
    }
}
