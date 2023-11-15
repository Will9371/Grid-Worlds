using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebServer
{
    string serverURL = "http://localhost:3000"; // Replace with your server's URL
    
    public Action<int[]> onResponse;

    public IEnumerator SendData(float[] observations, int[] actions)
    {
        ResponseData data = new ResponseData { input = observations, output = actions };
        yield return SendDataToServer("/sendData", data);
    }
    
    IEnumerator SendDataToServer(string route, object data)
    {
        // Convert the data to a JSON string
        var jsonData = JsonUtility.ToJson(data);

        // Set up the UnityWebRequest with POST method and the server URL
        var request = new UnityWebRequest(serverURL + route, "POST");
        var bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);

        // Attach the data to the request
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Set the request header for JSON content
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request
        yield return request.SendWebRequest();

        // Check for errors during the request
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Error: " + request.error);
        }
        // Request was successful, and you can handle the response here
        else
        {
            var response = request.downloadHandler.text;
            var responseData = JsonUtility.FromJson<ResponseData>(response);
            onResponse?.Invoke(responseData.output);
        }
    }

    [Serializable]
    public class ResponseData
    {
        public float[] input;
        public int[] output;
    }
}