using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebServer
{
    /// Server URL
    string serverURL = "http://localhost:3000";
    
    /// Server functions
    const string getParameters = "/getParameters";
    const string setParameters = "/setParameters";
    const string beginEpisode = "/beginEpisode";
    const string endEpisode = "/endEpisode";
    const string observe = "/observe";
    const string agentEvent = "/agentEvent";
    
    public Action<int[]> onGetActions;
    public Action<string> onGetParameters;
    
    public IEnumerator GetParameters() { yield return SendDataToServer(getParameters); }
    public IEnumerator SetParameters(WebParameters parameters) { yield return SendDataToServer(setParameters, parameters); }
    public IEnumerator BeginEpisode() { yield return SendDataToServer(beginEpisode); }
    public IEnumerator EndEpisode() { yield return SendDataToServer(endEpisode); }
    public IEnumerator SendEvent(GridWorldEvent value) { yield return SendDataToServer(agentEvent, new EventData { id = value.name }); }

    public IEnumerator SendData(float[] observations, int[] actions)
    {
        ResponseData data = new ResponseData { input = observations, output = actions };
        yield return SendDataToServer(observe, data);
    }
    
    IEnumerator SendDataToServer(string route, object data = null)
    {
        // Convert the data to a JSON string
        var jsonData = JsonUtility.ToJson(data);
        Debug.Log($"Sending data to server: {jsonData}");

        // Set up the UnityWebRequest with POST method and the server URL
        var request = new UnityWebRequest(serverURL + route, "POST");
        var bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);

        // Attach the data to the request
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Set the request header for JSON content
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // Check for errors during the request
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Error: " + request.error);
            yield break;
        }
        
        // Request was successful. Handle response, if applicable
        var response = request.downloadHandler.text;
        switch (route)
        {
            case observe:
                var responseData = JsonUtility.FromJson<ResponseData>(response);
                onGetActions?.Invoke(responseData.output);
                break;
            case getParameters:
                onGetParameters?.Invoke(response);
                break;
        }
    }

    [Serializable]
    public class ResponseData
    {
        public float[] input;
        public int[] output;
    }
    
    [Serializable]
    public class InputData
    {
        public string cellType;
        public string objectType;
        public string x;
        public string y;
    }
    
    [Serializable]
    public class EventData
    {
        public string id;
    }
}