using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class WatchTransformInEditor
{
    public bool watch = true;
    public float refreshRate = 0.1f;
    public Action<Vector3> onSetPosition;
    
    IEnumerator tick;
    
    public void OnValidate(MonoBehaviour mono, Action<Vector3> onSetPosition)
    {
        if (!watch || !mono.gameObject.activeInHierarchy)
            return;
        
        this.onSetPosition = onSetPosition;
    
        if (tick == null)
        {
            tick = Tick(mono.transform);
            mono.StartCoroutine(Tick(mono.transform));
        }
    }
    
    IEnumerator Tick(Transform transform)
    {
        var delay = new WaitForSecondsRealtime(refreshRate);
        while (watch && transform.gameObject.activeInHierarchy)
        {
            onSetPosition?.Invoke(transform.position);
            yield return delay;
        }
        tick = null;
    }    
}