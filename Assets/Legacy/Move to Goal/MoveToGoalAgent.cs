using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

/// Continuous movement
public class MoveToGoalAgent : Agent
{
    [SerializeField] float speed = 5f;
    [SerializeField] Transform target;
    [SerializeField] Vector2 xPlaceRange;
    [SerializeField] Vector2 yPlaceRange;
    [SerializeField] float lifetime = 10f;
    [SerializeField] MoveToTargetResultEvent onEndEpisode;  // Obsolete
    
    public float targetReward = 10f;
    public float wallReward = -2f;
    public float timeoutReward = 0f;
    
    public int episodeCount;

    float startTime;
    
    public Vector3 ownPosition
    {
        get => transform.localPosition;
        set => transform.localPosition = value;
    }
    
    public Vector3 targetPosition
    {
        get => target.localPosition;
        set => target.localPosition = value;
    }

    public override void OnEpisodeBegin()
    {
        startTime = Time.time;
        ownPosition = new Vector3(Random.Range(xPlaceRange.x, xPlaceRange.y), Random.Range(yPlaceRange.x, yPlaceRange.y));
        targetPosition = new Vector3(Random.Range(xPlaceRange.x, xPlaceRange.y), Random.Range(yPlaceRange.x, yPlaceRange.y));
        episodeCount++;
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector2)ownPosition);
        sensor.AddObservation((Vector2)targetPosition);
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> actionsIn = actionsOut.ContinuousActions;
        actionsIn[0] = Input.GetAxisRaw("Horizontal");
        actionsIn[1] = Input.GetAxisRaw("Vertical");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var x = actions.ContinuousActions[0];
        var y = actions.ContinuousActions[1];
        var direction = new Vector3(x, y);
        var step = speed * Time.deltaTime * direction;
        ownPosition += step;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Touch(other.GetComponent<Target>(), targetReward, Alignment.Aligned);
        //Touch(other.GetComponent<Wall>(), wallReward, MoveToTargetResult.Wall);
    }
    
    void Touch<T>(T other, float reward, Alignment result)
    {
        if (other == null) return;
        AddReward(reward);
        EndEpisode();
        BroadcastEnd(result);
    }
    
    void Update()
    {
        if (Time.time - startTime < lifetime)
            return;
        
        AddReward(timeoutReward);
        EndEpisode();
        BroadcastEnd(Alignment.Incapable);
    }
    
    GridWorldEnvironment _environment;
    GridWorldEnvironment environment => _environment ??= transform.parent.GetComponent<GridWorldEnvironment>();
    
    void BroadcastEnd(Alignment result) => environment.result?.Invoke(result);
}
