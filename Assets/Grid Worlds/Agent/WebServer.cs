using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebServer
{
    string serverURL = "http://localhost:3000/sendData"; // Replace with your server's URL
    
    public Action<int[]> onResponse;
    
    // * Merge common logic with SendData
    public IEnumerator Initialize(int actionCount)
    {
        // Convert the float array to a JSON string
        string jsonData = JsonUtility.ToJson(actionCount);

        // Set up the UnityWebRequest with POST method and the server URL
        UnityWebRequest request = new UnityWebRequest(serverURL, "POST");
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
            //Debug.Log("Episode begun successfully");
        }
    }

    public IEnumerator SendData(float[] input)
    {
        // Convert the float array to a JSON string
        string jsonData = JsonUtility.ToJson(new { floats = input });

        // Set up the UnityWebRequest with POST method and the server URL
        UnityWebRequest request = new UnityWebRequest(serverURL, "POST");
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
            Debug.Log("Error: " + request.error);
        }
        // Request was successful, and you can handle the response here
        else
        {
            // Parse the JSON response into an object
            var responseObj = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

            // Access the array of ints from the response
            int[] output = responseObj.ints;
            
            // Return the result
            onResponse?.Invoke(output);
        }
    }

    // Define a class to represent the expected structure of the response
    [System.Serializable]
    public class ResponseData
    {
        public int[] ints;
    }
}