using System.Collections;
using UnityEngine;

public class GridWorldWebAgent : MonoBehaviour
{
    [SerializeField] GridWorldAgent agent;
    [SerializeField] float stepDelay = .25f;
    [SerializeField] float beginEpisodeDelay = .5f;
    [SerializeField] float endEpisodeDelay = 0.5f;
    
    AgentObservations observations = new();
    WebServer server = new();
    
    bool active;
    
    [SerializeField] bool getParameters;
    
    void OnValidate()
    {
        if (getParameters)
        {
            getParameters = false;
            StartCoroutine(server.GetParameters());
        }
    }
    
    void Start()
    {
        agent.onEnd += OnEnd;
        server.onResponse = BeginReceiveActions;
        StartCoroutine(BeginEpisode());
    }
    
    void OnDestroy()
    {
        if (agent) agent.onEnd -= OnEnd;
    }
    
    IEnumerator BeginEpisode()
    {
        active = true;
        agent.Reset();
        yield return server.BeginEpisode();
        yield return new WaitForSeconds(beginEpisodeDelay);
        StartCoroutine(CollectObservations());
    }
    
    IEnumerator CollectObservations()
    {
        yield return new WaitForSeconds(stepDelay);
        var inputs = observations.GetValues(agent.CollectObservations());
        var actions = new[] { 2, 2 };  // * Get from GridWorldAgent
        yield return server.SendData(inputs, actions);
    }
    
    void BeginReceiveActions(int[] actions) => StartCoroutine(ReceiveActions(actions));
    
    
    IEnumerator ReceiveActions(int[] actions)
    {
        if (!active) yield break;
        agent.OnActionReceived(actions);
        StartCoroutine(CollectObservations());
    }
    
    void OnEnd() => StartCoroutine(EndEpisode());
    IEnumerator EndEpisode()
    {
        active = false;
        yield return server.EndEpisode();
        yield return new WaitForSeconds(endEpisodeDelay);
        StartCoroutine(BeginEpisode());
    }
}
