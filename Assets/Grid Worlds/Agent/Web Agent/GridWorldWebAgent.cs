using System.Collections;
using UnityEngine;

public class GridWorldWebAgent : MonoBehaviour, IAgent
{
    GridWorldAgent agent;
    [SerializeField] WebParametersInfo parameters;
    
    AgentObservations observations = new();
    WebServer server = new();
    
    [ReadOnly, SerializeField] bool active;
    
    enum ControlSource { AI, Human }
    [SerializeField] ControlSource controlSource;
    bool isAI => controlSource == ControlSource.AI;
    
    bool beginFlag = true;
    bool stepFlag = true;
    bool endFlag = true;
    public void SetBeginFlag(bool value) => beginFlag = value;
    public void SetStepFlag(bool value) => stepFlag = value;
    public void SetEndFlag(bool value) => endFlag = value;
    
    public void Inject(GridWorldAgent agent) => this.agent = agent;
    
    void Start()
    {
        server.onGetActions = BeginReceiveActions;
        
        switch (controlSource)
        {
            case ControlSource.AI: StartCoroutine(InitializeAI()); break;
            case ControlSource.Human: StartCoroutine(HumanControl()); break;
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
        //agent.Reset();
        
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
        
        yield return server.SendData(inputs, actions);
    }
    
    void BeginReceiveActions(int[] actions) => StartCoroutine(ReceiveActions(actions));
    IEnumerator ReceiveActions(int[] actions)
    {
        if (!active || !isAI) yield break;
        agent.OnActionReceived(actions);
        StartCoroutine(CollectObservations());
    }
    
    public void AddEvent(GridWorldEvent info) => StartCoroutine(server.SendEvent(info));
    
    public void End() => StartCoroutine(EndEpisode());
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
                //Debug.Log(input[0]);
                agent.OnActionReceived(input);
            }
            
            while (stepFlag) yield return null;
            stepFlag = true;
        }
    }
}
