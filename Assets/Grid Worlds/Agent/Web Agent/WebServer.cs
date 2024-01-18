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
    const string query = "/query";
    
    public Action<string[]> onGetActions;
    public Action<string> onGetParameters;
    public Action<string> onGetQueryOutput;
    
    public IEnumerator GetParameters() { yield return SendDataToServer(getParameters); }
    public IEnumerator SetParameters(WebParameters parameters) { yield return SendDataToServer(setParameters, parameters); }
    public IEnumerator BeginEpisode() { yield return SendDataToServer(beginEpisode); }
    public IEnumerator EndEpisode() { yield return SendDataToServer(endEpisode); }
    public IEnumerator SendEvent(GridWorldEvent value) { yield return SendDataToServer(agentEvent, new EventData { id = value.name }); }

    public IEnumerator SendObservations(ObservationData observations, ResponseData response)
    {
        IOData data = new IOData { input = observations, output = response };
        yield return SendDataToServer(observe, data);
    }
    
    IEnumerator SendDataToServer(string route, object data = null)
    {
        // Convert the data to a JSON string
        var jsonData = JsonUtility.ToJson(data);
        //Debug.Log($"Sending data to server: {jsonData}");

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
        //Debug.Log($"{route}, {response}");
        
        if (route == getParameters)
            onGetParameters?.Invoke(response);
        else if (route == observe || route == query)
        {
            var responseData = JsonUtility.FromJson<ResponseData>(response);
            
            if (responseData.mode == "action")
                onGetActions?.Invoke(responseData.actions);
            else if (responseData.mode == "query")
                yield return SendDataToServer(query, data);
        }
    }

    [Serializable]
    public class IOData
    {
        public ObservationData input;
        public ResponseData output;
    }
    
    /// Obsolete, merge into ObservationData
    [Serializable]
    public class EventData
    {
        public string id;
    }
}

[Serializable]
public struct ResponseData
{
    public string mode;
    public string[] actions;
    
    public ResponseData(string mode, string[] actions)
    {
        this.mode = mode;
        this.actions = actions;
    }
}