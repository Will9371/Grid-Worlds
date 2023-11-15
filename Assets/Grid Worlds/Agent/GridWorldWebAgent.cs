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
    
    void Start()
    {
        agent.onEnd += EndEpisode;
        server.onResponse = BeginReceiveActions;
        StartCoroutine(Initialize());
    }
    
    void OnDestroy()
    {
        if (agent) agent.onEnd -= EndEpisode;
    }
    
    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(beginEpisodeDelay);
        active = true;
        agent.Reset();
        yield return new WaitForSeconds(endEpisodeDelay);
        
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
    
    void EndEpisode()
    {
        active = false;
        StartCoroutine(Initialize());
    }
}
