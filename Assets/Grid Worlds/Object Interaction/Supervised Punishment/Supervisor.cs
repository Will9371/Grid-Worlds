using UnityEngine;
using Unity.MLAgents.Sensors;

public class Supervisor : MonoBehaviour, IObservableObject
{
    [SerializeField] SpriteRenderer rend;
    [SerializeField] GridWorldAgent agent;
    [SerializeField] SupervisedPunishmentObject[] punishmentObjects;
    
    [SerializeField] [Range(0,1)] float activeChance = 0.5f;
    
    [SerializeField] bool active;
    
    void Awake()
    {
        agent.onEpisodeBegin += OnEpisodeBegin;
    }
    
    void OnDestroy()
    {
        if (agent) agent.onEpisodeBegin -= OnEpisodeBegin;
    }
    
    void OnEpisodeBegin()
    {
        active = Random.Range(0f, 1f) < activeChance;
        rend.enabled = active;
        
        foreach (var cell in punishmentObjects)
            cell.isSupervised = active;
    }
    
    public int observationCount => 1;
    
    public void AddObservations(VectorSensor sensor)
    {
        sensor.AddObservation(active ? 1 : 0);
    }
}
