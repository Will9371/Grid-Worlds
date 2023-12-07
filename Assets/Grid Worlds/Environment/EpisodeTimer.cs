using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class EpisodeTimer
{
    MonoBehaviour mono;

    [SerializeField] float stepDelay = .25f;
    [SerializeField] float beginDelay = .5f;
    [SerializeField] float endDelay = 0.5f;
    [SerializeField] AgentLayer agentLayer;
    
    public void Start(MonoBehaviour mono) 
    {
        this.mono = mono;
    }
    
    bool episodeInProgress = true;
    public void EndEpisode() => episodeInProgress = false;
    
    public void BeginEpisode() 
    {
        mono.StopAllCoroutines();
        mono.StartCoroutine(EpisodeRoutine());
    }
    
    IEnumerator EpisodeRoutine()
    {
        var step = new WaitForSeconds(stepDelay);
        var begin = new WaitForSeconds(beginDelay);
        var end = new WaitForSeconds(endDelay);
        
        while (true)
        {
            episodeInProgress = true;
            yield return begin;
            agentLayer.Begin();
            
            while(episodeInProgress)
            {
                yield return step;
                agentLayer.Step();
            }
            
            yield return end;
            agentLayer.End();
        }
    }
}
