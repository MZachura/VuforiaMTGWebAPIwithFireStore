#pragma warning disable IDE0058 
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class ModelTargetUploader : MonoBehaviour
{
    private static ModelTargetUploader instance;

    [System.Serializable]
    public class AccessTokenData
    {
        public string access_token;
        public string token_type;
        public int expires_in;
    }



    private readonly string vuforiaEndpoint = "https://vws.vuforia.com/modeltargets/advancedDatasets";
    private const string token = "VuforiaToken";
    private const string firebaseUrl = "FirebaseUrl";
    private const string VuforiaUuid = "VuforiaUuid";
    public UnityEngine.UI.Button createDatasetButton;


    private void Start()
    {
        instance = this;
        createDatasetButton.onClick.AddListener(OnCreateDatasetButtonClick);
    }
    private void OnCreateDatasetButtonClick()
    {
        FileUploader.LoadFile();
    }
    public static void StepOne(string path)
    {
        CloudFirestore.SavingStarter(path);
    }
    public static void StepTwo()
    {
        instance.CreateDataset();
    }
    public void CreateDataset()
    {
        //Check if the token exists in cache
        if (PlayerPrefs.HasKey(token))
        {
            string accessTokenBefore = PlayerPrefs.GetString(token);


            AccessTokenData tokenData = JsonUtility.FromJson<AccessTokenData>(accessTokenBefore);
            string accessToken = tokenData.access_token;

            Debug.Log("Access Token: " + accessToken);
            if (PlayerPrefs.HasKey(firebaseUrl))
            {
                string Firebaseurl = PlayerPrefs.GetString(firebaseUrl);


                Debug.Log("firebaseUrl: " + Firebaseurl);
                StartCoroutine(SendRequest(accessToken, Firebaseurl));
            }
            else
            {
                Debug.LogError("No firebaseUrl");
            }
        }
        else
        {
            Debug.LogError("Access Token not found in cache. Make sure to obtain the token first.");
        }
    }

    private IEnumerator SendRequest(string token, string firebaseUrl)
    {
        // Prepare the request body
        string requestString = GetRequestBody(firebaseUrl);
        //Debug.Log(requestString);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestString);

        // Define the URL for the request
        string url = vuforiaEndpoint;

        // Create a UnityWebRequest instance using POST method
        UnityWebRequest request = UnityWebRequest.Post(url, new WWWForm());

        // Set the Authorization header with the provided token
        string authorizationHeader = $"Bearer {token}";
        request.SetRequestHeader("Authorization", authorizationHeader);

        // Attach the raw request body to the UnityWebRequest
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // Set the handler to receive the response data
        request.downloadHandler = new DownloadHandlerBuffer();

        // Set the Content-Type header
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for the response
        yield return request.SendWebRequest();
        //yield return null;

        // Check for errors in the response
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.LogError("Post request failed. Error: " + request.error);
            Debug.LogError("400 Bad Request Error. Response: " + request.downloadHandler.text);
        }
        else
        {
            // Request was successful, log the response
            Debug.Log("Post request successful!");
            Debug.Log("Response: " + request.downloadHandler.text);
            PlayerPrefs.SetString(VuforiaUuid, request.downloadHandler.text);
            PlayerPrefs.Save();
        }


    }

    private string GetRequestBody(string firebaseUrl)
    {
        // get the firebase links as a variable
        string jsonString = /*lang=json,strict*/

            @"{
        ""name"": ""NewTestCad"",
        ""preserveCadModel"": false,
        ""targetSdk"": ""10.13.3"",
        ""models"": [
            {
                ""name"":""NewTestCad"",
                ""cadDataFormat"": ""STL"",
                 ""realisticAppearance"": ""false"",
                 ""upVector"": [ 0, 0, 1 ],
                ""uniformScale"": 1,
                ""optimizeTrackingFor"":""default"",
                ""automaticColoring"":""always"",
                 ""cadDataUrl"": """ + firebaseUrl + @""",
                ""views"": [{

                        ""name"": ""NewView"",
                        ""recognitionRangesPreset"":""FULL_360"",
                        ""targetExtentPreset"":""FULL_MODEL"",
                        ""layout"":""portrait""
                    }]
                
            }]

            }";
        return jsonString;
    }

}

