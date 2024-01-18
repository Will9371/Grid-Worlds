using System;
using System.Collections;
using UnityEngine;

public class GridWorldWebAgent : MonoBehaviour, IAgent
{
    GridWorldAgent agent;
    [SerializeField] WebParametersInfo parameters;
    
    //AgentObservations observations => agent.observations;
    WebServer server = new();
    
    [ReadOnly, SerializeField] bool active;
    
    enum ControlSource { AI, Human }
    [SerializeField] ControlSource controlSource;
    bool isAI => controlSource == ControlSource.AI;
    
    public void Inject(GridWorldAgent agent) => this.agent = agent;
    
    public void Begin()
    {
        switch (controlSource)
        {
            case ControlSource.AI: StartCoroutine(BeginEpisodeAsAI()); break;
            case ControlSource.Human: agent.BeginComplete(); break;
        }
    }
    
    IEnumerator BeginEpisodeAsAI()
    {
        if (!isAI) yield break;
        
        server.onGetActions = BeginReceiveActions;
        yield return server.SetParameters(parameters.data);
        
        active = true;
        yield return server.BeginEpisode();
        agent.BeginComplete();
    }
    
    public void Step() 
    {
        switch (controlSource)
        {
            case ControlSource.AI: StartCoroutine(CollectObservations()); break;
            case ControlSource.Human: agent.ApplyPlayerControl(); break;
        }
    }
    
    IEnumerator CollectObservations()
    {
        var input = agent.RefreshObservations();
        var output = new ResponseData("action", agent.ActionNames());
        yield return server.SendObservations(input, output);
    }
    
    void BeginReceiveActions(string[] actions) => StartCoroutine(ReceiveActions(Statics.ActionNamesToIds(actions)));
    IEnumerator ReceiveActions(int[] actions)
    {
        if (!active || !isAI) yield break;
        agent.OnActionReceived(actions);
    }
    
    public void AddEvent(GridWorldEvent info) 
    {
        if (controlSource == ControlSource.AI)
            StartCoroutine(server.SendEvent(info));
    }
    
    public void End() => StartCoroutine(EndEpisode());
    IEnumerator EndEpisode()
    {
        //Debug.Log("WebAgent.EndEpisode()");
        active = false;
        
        if (controlSource == ControlSource.AI)
            yield return server.EndEpisode();
        
        agent.EndComplete();
    }
}
