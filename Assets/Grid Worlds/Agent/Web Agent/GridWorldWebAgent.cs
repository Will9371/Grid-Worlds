using System.Collections;
using UnityEngine;

public class GridWorldWebAgent : MonoBehaviour
{
    [SerializeField] GridWorldAgent agent;
    [SerializeField] WebParametersInfo parameters;
    
    AgentObservations observations = new();
    WebServer server = new();
    
    [ReadOnly, SerializeField] bool active;
    
    enum ControlSource { AI, Human }
    [SerializeField] ControlSource controlSource;
    bool isAI => controlSource == ControlSource.AI;
    
    [Header("Debug")]
    [ReadOnly] public bool beginFlag = true;
    [ReadOnly] public bool stepFlag = true;
    [ReadOnly] public bool endFlag = true;
    void ClearBeginFlag() => beginFlag = false;
    void ClearStepFlag() => stepFlag = false;
    void ClearEndFlag() => endFlag = false;
    
    void Start()
    {
        agent.onEnd += OnEnd;
        agent.onEvent += AgentEvent;
        server.onGetActions = BeginReceiveActions;
        
        agent.onClearBeginFlag = ClearBeginFlag;
        agent.onClearStepFlag = ClearStepFlag;
        agent.onClearEndFlag = ClearEndFlag;
        
        switch (controlSource)
        {
            case ControlSource.AI: StartCoroutine(InitializeAI()); break;
            case ControlSource.Human: StartCoroutine(HumanControl()); break;
        }
    }
    
    void OnDestroy()
    {
        if (agent)
        {
            agent.onEnd -= OnEnd;
            agent.onEvent -= AgentEvent;
        }
    }
    
    IEnumerator InitializeAI()
    {
        yield return server.SetParameters(parameters.data);
        StartCoroutine(BeginEpisodeAsAI());
    }
    
    IEnumerator BeginEpisodeAsAI()
    {
        if (!isAI) yield break;
        active = true;
        agent.Reset();
        
        yield return server.BeginEpisode();
        
        while (beginFlag) yield return null;
        beginFlag = true;
        
        StartCoroutine(CollectObservations());
    }
    
    IEnumerator CollectObservations()
    {
        while (stepFlag) yield return null;
        stepFlag = true;
        
        var inputs = observations.GetValues(agent.CollectObservations());
        var actions = agent.ActionSpace();
        //new[] { 4 };  // * Get from GridWorldAgent, movement (can be {2, 2})
        
        yield return server.SendData(inputs, actions);
    }
    
    void BeginReceiveActions(int[] actions) => StartCoroutine(ReceiveActions(actions));
    IEnumerator ReceiveActions(int[] actions)
    {
        if (!active || !isAI) yield break;
        agent.OnActionReceived(actions);
        StartCoroutine(CollectObservations());
    }
    
    void AgentEvent(GridWorldEvent info) => StartCoroutine(server.SendEvent(info));
    
    void OnEnd() => StartCoroutine(EndEpisode());
    IEnumerator EndEpisode()
    {
        if (isAI) yield break;
        active = false;
        
        yield return server.EndEpisode();
        
        while (endFlag) yield return null;
        endFlag = true;
        
        StartCoroutine(BeginEpisodeAsAI());
    }
    
    IEnumerator HumanControl()
    {
        while (true)
        {
            if (agent.alive)
            {
                var input = agent.PlayerControl();
                agent.OnActionReceived(input);
            }
            
            while (stepFlag) yield return null;
            stepFlag = true;
        }
    }
}
