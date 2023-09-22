using UnityEngine;

public interface IReward { float reward { get; set; } }

public class Target : MonoBehaviour, IReward
{ 
    public float _reward;
    public bool endEpisodeOnTouch;
    
    public float reward
    {
        get => _reward;
        set => _reward = value;
    }
}
