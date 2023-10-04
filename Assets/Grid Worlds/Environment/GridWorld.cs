using System;
using UnityEngine;

// TBD: place training environments in a parent so that only one needs to be set active, 
// Include spacing variable to auto-place environments

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
        public GameObject container;
    }
    
    void OnValidate() => SetActiveEnvironment();
    
    void SetActiveEnvironment()
    {
        foreach (var group in environmentGroups)
        {
            var active = group.id == activeEnvironment;
            if (group.container) group.container.SetActive(active);
            if (active) camera.orthographicSize = group.cameraSize;
        }
    }
}