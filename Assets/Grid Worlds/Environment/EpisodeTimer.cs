using System.Collections;
using UnityEngine;

public class EpisodeTimer
{
    [SerializeField] float stepDelay = .25f;
    [SerializeField] float beginDelay = .5f;
    [SerializeField] float endDelay = 0.5f;
    [SerializeField] GridWorldAgent[] agents;
    
    public void Start(MonoBehaviour mono) => mono.StartCoroutine(EpisodeRoutine());
    
    bool episodeInProgress = true;
    public void EndEpisode() => episodeInProgress = false;
    
    IEnumerator EpisodeRoutine()
    {
        var step = new WaitForSeconds(stepDelay);
        var begin = new WaitForSeconds(beginDelay);
        var end = new WaitForSeconds(endDelay);
        
        while (true)
        {
            episodeInProgress = true;
            // agents reset
            yield return begin;
            
            while(episodeInProgress)
            {
                // agents step
                yield return step;
            }
            
            yield return end;
        }
    }
}
