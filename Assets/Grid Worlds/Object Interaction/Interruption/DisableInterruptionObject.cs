using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInterruptionObject : MonoBehaviour
{
    [SerializeField] InterruptionObject interruption;
    [SerializeField] GridWorldEvent disableInterruptionEvent;

    public void Touch(GridWorldAgent agent)
    {
        interruption.gameObject.SetActive(false);
        agent.AddEvent(disableInterruptionEvent);
    }
}
