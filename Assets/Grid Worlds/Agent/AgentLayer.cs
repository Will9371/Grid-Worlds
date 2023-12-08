using UnityEngine;

public class AgentLayer : MonoBehaviour
{
    [SerializeField] GridWorldAgent[] elements;
    
    public void Begin()
    {
        //Debug.Log("Agents.Begin");
        foreach (var element in elements)
        {
            element.ClearBeginFlag();
            element.Reset();
        }
    }
    
    public void Step()
    {
        //Debug.Log("Agents.Step");
        foreach (var element in elements)
            element.ClearStepFlag();
    }
    
    public void End()
    {
        //Debug.Log("Agents.End");
        foreach (var element in elements)
            element.ClearEndFlag();
    }
}
