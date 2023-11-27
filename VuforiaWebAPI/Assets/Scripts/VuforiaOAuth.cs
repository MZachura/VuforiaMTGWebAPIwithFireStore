#pragma warning disable IDE0058 
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class VuforiaOAuth : MonoBehaviour
{
    private const string AUTH_URL = "https://vws.vuforia.com/oauth2/token";
    private const string TokenKey = "VuforiaToken"; // Key to store the token in cache

    public const string CLIENT_ID = "XPZSSOJJZMTSJXQNZYZ5E";
    private const string CLIENT_SECRET = "dXMalQzsr9xUoM73XsnrjW9ul8W8vAXc5";
    //I should probably have the client credentials somewhere else... but whatever....
    private void Start()
    {
        StartCoroutine(GetAccessToken());
    }

    private IEnumerator GetAccessToken()
    {


        string url = AUTH_URL;
        UnityWebRequest request = UnityWebRequest.Post(url, new WWWForm());

        // Set the Authorization header with client credentials
        string authorizationHeader = $"Basic {Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{CLIENT_ID}:{CLIENT_SECRET}"))}";
        request.SetRequestHeader("Authorization", authorizationHeader);

        // Request body
        string requestBody = "grant_type=client_credentials";
        byte[] bodyData = System.Text.Encoding.UTF8.GetBytes(requestBody);
        request.uploadHandler = new UploadHandlerRaw(bodyData);

        yield return request.SendWebRequest(); // Send the request and wait for a response

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to retrieve OAuth token: " + request.error);
        }
        else
        {
            string response = request.downloadHandler.text;
            // Process the response JSON data here
            Debug.Log("Access Token: " + response);

            // Save the token to cache
            PlayerPrefs.SetString(TokenKey, response);
            PlayerPrefs.Save();
        }
    }
}
