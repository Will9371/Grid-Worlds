using System.Collections;
using UnityEngine;

/// Step => Observations => Server => Simulated Actions => Calculate Movement => Display Simulacrum Movement =>
/// Step => Observations => Server => Real Actions => Apply Movement
public class GridWorldWebAgent : MonoBehaviour, IAgent
{
    GridWorldAgent agent;
    [SerializeField] WebParametersInfo parameters;
    [SerializeField] EnvironmentUI ui;
    [ReadOnly, SerializeField] bool active;
    
    enum ControlSource { AI, Human }
    [SerializeField] ControlSource controlSource;
    bool isAI => controlSource == ControlSource.AI;
    
    WebServer server = new();
    
    public void Inject(GridWorldAgent agent) => this.agent = agent;
    
    public void Begin()
    {
        ui.ResetReward();

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
        server.onGetQuery = BeginReceiveSimulatedActions;
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
    
    public void SimulatedStep() 
    {
        switch (controlSource)
        {
            case ControlSource.AI: StartCoroutine(CollectSimulatedObservations()); break;
            case ControlSource.Human: agent.ApplyPlayerControl(); break;
        }
        
        ui.AddReward(-simulatedReward);
        simulatedReward = 0f;
    }
    
    IEnumerator CollectObservations()
    {
        var input = agent.RefreshObservations();
        var output = new ResponseData("action", agent.ActionNames());
        yield return server.SendObservations(input, output);
    }
    
    /// TBD: get observations for each possibility branch
    IEnumerator CollectSimulatedObservations()
    {
        var input = agent.RefreshObservations();
        
        var simulation = new SimObservationData(input);
        var simulations = new SimObservationData[1];
        simulations[0] = simulation;
        
        var output = new ResponseData("action", agent.ActionNames());
        yield return server.SendSimulation(simulations, output);
    }
    
    void BeginReceiveActions(string[] actions) => StartCoroutine(ReceiveActions(Statics.ActionNamesToIds(actions)));
    IEnumerator ReceiveActions(int[] actions)
    {
        agent.simulated = false;
        if (!active || !isAI) yield break;
        agent.OnActionReceived(actions);
    }
    
    void BeginReceiveSimulatedActions(string[] actions) => StartCoroutine(ReceiveSimulatedActions(Statics.ActionNamesToIds(actions)));
    IEnumerator ReceiveSimulatedActions(int[] actions)
    {
        agent.simulated = true;
        if (!active || !isAI) yield break;
        agent.OnActionReceived(actions, true);
    }
    
    public void AddEvent(GridWorldEvent info) 
    {
        if (controlSource == ControlSource.AI)
            StartCoroutine(server.SendEvent(info));
    }
    
    float simulatedReward;
    
    public void AddTouchId(string value)
    {
        //if (agent.simulated) return;
        var reward = parameters.GetReward(value);
        if (agent.simulated) simulatedReward += reward;
        ui.AddReward(reward);
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
