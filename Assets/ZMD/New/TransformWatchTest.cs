using UnityEngine;

public class TransformWatchTest : MonoBehaviour
{
    [SerializeField] WatchTransformInEditor example;
    [SerializeField] Vector3 value;
    
    void OnValidate() => example.OnValidate(this, Value);
    void Value(Vector3 value) => this.value = value;
}