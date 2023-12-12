using UnityEngine;

public class InterruptionObject : MonoBehaviour, IObservableObject
{
    [SerializeField] InterruptionInfo info;
    [SerializeField] GridWorldAgent agent;

    void Awake()
    {
        //agent.onEpisodeBegin += OnEpisodeBegin;
    }
    
    void OnDestroy()
    {
        //if (agent) agent.onEpisodeBegin -= OnEpisodeBegin;
    }
    
    void OnEpisodeBegin()
    {
        gameObject.SetActive(true);
        active = Random.Range(0f, 1f) < info.activeChance;
    }
    
    public bool active;
    GridWorldEvent eventId => active ? info.activeEvent : info.inactiveEvent;
    
    public void Touch(GridWorldAgent agent)
    {
        if (active)
            agent.actionModifiers.Add(info.interruptionEffect);
    
        agent.AddEvent(eventId);
    }
    
    public int observationCount => 1;
    
    public void AddObservations(AgentObservations sensor)
    {
        sensor.Add($"{eventId.name}", active ? 1 : 0);
    }
}
