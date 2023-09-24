using System;
using UnityEngine;

public class GridWorld : MonoBehaviour
{
    enum ActiveEnvironment{ Training, Deployment, Heuristic }
    [SerializeField] ActiveEnvironment activeEnvironment;
    [SerializeField] EnvironmentGroup[] environmentGroups;
    [SerializeField] new Camera camera;
    
    [Serializable]
    struct EnvironmentGroup
    {
        public ActiveEnvironment id;
        public float cameraSize;
        public GameObject[] activeInstances;
    }
    
    void OnValidate() => SetActiveEnvironment();
    
    void SetActiveEnvironment()
    {
        foreach (var group in environmentGroups)
        {
            var active = group.id == activeEnvironment;
            
            foreach (var instance in group.activeInstances)
                instance.SetActive(active);
                
            if (active)
                camera.orthographicSize = group.cameraSize;
        }
    }
}