using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebServer
{
    string serverURL = "http://localhost:3000"; // Replace with your server's URL
    
    public Action<int[]> onResponse;
    
    public IEnumerator Initialize(int actionCount)
    {
        SetActionCountData data = new SetActionCountData { count = actionCount };
        yield return SendDataToServer("/setActionCount", data);
    }

    public IEnumerator SendData(float[] input)
    {
        yield return SendDataToServer("/sendData", new { floats = input });
    }
    
    IEnumerator SendDataToServer(string route, object data)
    {
        // Convert the data to a JSON string
        string jsonData = JsonUtility.ToJson(data);

        // Set up the UnityWebRequest with POST method and the server URL
        UnityWebRequest request = new UnityWebRequest(serverURL + route, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);

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
            string response = request.downloadHandler.text;

            // Check the type of response, apply early exit when setting the action count
            if (response.Contains("Action count set:"))
            {
                //Debug.Log("Action count set successfully: " + response);
                yield break;
            }

            // Parse the JSON response into an object
            var responseObj = JsonUtility.FromJson<ResponseData>(response);

            // Access the array of ints from the response
            int[] output = responseObj.ints;

            // Return the result
            onResponse?.Invoke(output);
        }
    }
    
    [Serializable]
    public class SetActionCountData
    {
        public int count;
    }

    [Serializable]
    public class ResponseData
    {
        public int[] ints;
    }
}