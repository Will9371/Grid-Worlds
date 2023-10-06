using System;
using System.Collections;
using UnityEngine;

// TBD: place training environments in a parent so that only one needs to be set active, 
// Include spacing variable to auto-place environments

public class GridWorld : MonoBehaviour
{
    enum ActiveEnvironment{ Training, Deployment, Heuristic }
    [SerializeField] ActiveEnvironment activeEnvironment;
    [SerializeField] EnvironmentGroup[] environmentGroups;
    
    [SerializeField] GameObject environment;
    [SerializeField] bool refreshEnvironments;
    
    [Header("Training")]
    [SerializeField] Vector2 size;
    [SerializeField] Vector2 buffer;
    [SerializeField] bool refreshTraining;
    
    [Header("Auto-Configured")]
    [ReadOnly] [SerializeField] new Camera camera;
    [ReadOnly] [SerializeField] GameObject trainingContainer;
    [ReadOnly] [SerializeField] EnvironmentPlacer training;
    
    [Serializable]
    struct EnvironmentGroup
    {
        [ReadOnly] public ActiveEnvironment id;
        public float cameraSize;
        [HideInInspector] public GameObject container;
    }
    
    void OnValidate() 
    {
        if (!environment) return;
    
        SetActiveEnvironment();

        if (refreshEnvironments)
        {
            refreshEnvironments = false;
            activeEnvironment = ActiveEnvironment.Training;
            StartCoroutine(RefreshEnvironments());
        }
        
        if (refreshTraining)
        {
            refreshTraining = false;
            if (training)
            {
                training.size = size;
                training.buffer = buffer;
                training.OnValidate();
                training.BeginRefresh();
            }
        }
    }

    void SetActiveEnvironment()
    {
        foreach (var group in environmentGroups)
        {
            var active = group.id == activeEnvironment;
            if (group.container) group.container.SetActive(active);
            if (active) camera.orthographicSize = group.cameraSize;
        }
    }
    
    IEnumerator RefreshEnvironments()
    {
        for (int i = transform.childCount - 1; i > 0; i--)
        {
            var child = transform.GetChild(i);
            if (child.GetComponent<Camera>()) continue;
            StartCoroutine(DestroyInstance(child.gameObject));
        }
        yield return null;
        
        StartCoroutine(GenerateInstance(environment, transform));
        StartCoroutine(GenerateInstance(environment, transform));
        StartCoroutine(GenerateInstance(trainingContainer, transform));
        yield return null;

        training = GetComponentInChildren<EnvironmentPlacer>();
        training.transform.SetSiblingIndex(2);
        training.prefab = environment;
        training.size = size;
        training.buffer = buffer;
        training.OnValidate();
        training.BeginRefresh();
        
        transform.GetChild(1).name = "Heuristic";
        transform.GetChild(2).name = "Training";
        transform.GetChild(3).name = "Deployment";
        
        environmentGroups[0].container = transform.GetChild(1).gameObject;
        environmentGroups[1].container = transform.GetChild(2).gameObject;
        environmentGroups[2].container = transform.GetChild(3).gameObject;
        
        SetActiveEnvironment();
    }
    
    IEnumerator GenerateInstance(GameObject prefab, Transform container)
    {
        yield return null;
        Instantiate(prefab, container);
    }
    
    IEnumerator DestroyInstance(GameObject value)
    {
        yield return null;
        DestroyImmediate(value);
    }
}