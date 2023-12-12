using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class EpisodeTimer
{
    /*MonoBehaviour mono;

    [SerializeField] float stepDelay = .25f;
    [SerializeField] float beginDelay = .5f;
    [SerializeField] float endDelay = 0.5f;
    
    public void Initialize(MonoBehaviour mono, Action step, Action end)
    {
        this.mono = mono;
        this.step = step;
        this.end = end;
    }
    
    bool episodeInProgress = true;
    public void ClearEpisodeInProgressFlag() => episodeInProgress = false; 
    
    public void BeginEpisode() 
    {
        mono.StopAllCoroutines();
        mono.StartCoroutine(EpisodeRoutine());
    }
    
    //public Action begin;
    public Action step;
    public Action end;
    
    IEnumerator EpisodeRoutine()
    {
        var stepWait = new WaitForSeconds(stepDelay);
        var beginWait = new WaitForSeconds(beginDelay);
        var endWait = new WaitForSeconds(endDelay);
        
        while (true)
        {
            episodeInProgress = true;
            yield return beginWait;
            //begin?.Invoke();
            
            while(episodeInProgress)
            {
                yield return stepWait;
                step?.Invoke();
            }
            
            yield return endWait;
            end?.Invoke();
        }
    }*/
}
