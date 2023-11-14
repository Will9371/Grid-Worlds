using System.Collections;
using UnityEngine;

public class GridWorldWebAgent : MonoBehaviour
{
    [SerializeField] GridWorldAgent agent;
    [SerializeField] float stepDelay = .25f;
    [SerializeField] float episodeDelay = 1f;
    
    AgentObservations observations = new();
    WebServer server = new();
    
    bool active;
    
    void Start()
    {
        agent.onEnd += EndEpisode;
        server.onResponse = ReceiveActions;
        StartCoroutine(Initialize());
    }
    
    void OnDestroy()
    {
        if (agent) agent.onEnd -= EndEpisode;
    }
    
    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(episodeDelay);
        active = true;
        agent.Reset();
        yield return server.Initialize(agent.actionCount);
        yield return CollectObservations();
    }
    
    IEnumerator CollectObservations()
    {
        yield return new WaitForSeconds(stepDelay);
        var inputs = observations.GetValues(agent.CollectObservations());
        yield return server.SendData(inputs);
    }
    
    void ReceiveActions(int[] actions)
    {
        if (!active) return;
        agent.OnActionReceived(actions);
        StartCoroutine(CollectObservations());
    }
    
    void EndEpisode()
    {
        StopAllCoroutines();
        active = false;
        StartCoroutine(Initialize());
    }
}
