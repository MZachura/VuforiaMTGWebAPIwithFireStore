#pragma warning disable IDE0058 
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System;
using Vuforia;

public class VuforiaGetRequests : MonoBehaviour
{
    public TMP_Text progressText;
    [System.Serializable]
    public class AccessTokenData
    {
        public string access_token;
        public string token_type;
        public int expires_in;
    }
    [System.Serializable]
    public class ProgressData
    {
        public string uuid;
        public string status;
    }

    [System.Serializable]
    public class VuforiaUuid
    {
        public string uuid;
    }
    //"https://vws.vuforia.com/modeltargets/datasets/$uuid/status"
    private readonly string vuforiaEndpoint = "https://vws.vuforia.com/modeltargets/advancedDatasets";
    private const string token = "VuforiaToken";
    private const string uuid = "VuforiaUuid";
    public UnityEngine.UI.Button checkIfItsDoneButton;
    //public UnityEngine.UI.Button deleteButton;
    public UnityEngine.UI.Button downloadButton;
    // Start is called before the first frame update
    void Start()
    {
        checkIfItsDoneButton.onClick.AddListener(OnCheckIfItsDoneButtonClick);
        downloadButton.onClick.AddListener(OnDownloadButtonClick);
        //deleteButton.onClick.AddListener(OnDeleteButtonClick);

    }
    private void OnCheckIfItsDoneButtonClick()
    {
        var AuthUuid = ObtainTokenAndUuid();
        if (AuthUuid != null)
        {
            //Debug.Log($"List: {AuthUuid[0]},{AuthUuid[1]}");
            StartCoroutine(CheckStatusRequest(AuthUuid[0].ToString(), AuthUuid[1].ToString()));
        }


    }
    public static string getBetween(string strSource, string strStart, string strEnd)
    {
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            int Start, End;
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - (Start+1));
        }

        return "";
    }
    private void OnDownloadButtonClick()
    {
        var AuthUuid = ObtainTokenAndUuid();
        if (AuthUuid != null)
        {
            //Debug.Log($"List: {AuthUuid[0]},{AuthUuid[1]}");
            StartCoroutine(DownloadZipFromVuforia(AuthUuid[0].ToString(), AuthUuid[1].ToString()));
        }
    }

    private void OnDeleteButtonClick()
    {
        var AuthUuid = ObtainTokenAndUuid();
        if (AuthUuid != null)
        {
            //Debug.Log($"List: {AuthUuid[0]},{AuthUuid[1]}");
            StartCoroutine(DeleteDatasetOnVuforia(AuthUuid[0].ToString(), AuthUuid[1].ToString()));
        }
    }
    public IEnumerator CheckStatusRequest(string token, string uuid)
    {
        string authorizationHeader = $"Bearer {token}";
        string url = $"{vuforiaEndpoint}/{uuid}/status";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", authorizationHeader);
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.LogError("Get request failed. Error: " + request.error);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Get method successful");
            byte[] results = request.downloadHandler.data;
            string responseText = Encoding.UTF8.GetString(results);
            ProgressData progressData = JsonUtility.FromJson<ProgressData>(responseText);
            string progressTextFromJson = progressData.status;
            progressText.text = progressTextFromJson;
            Debug.Log("Response: " + responseText);
        }
    }
    public IEnumerator DownloadZipFromVuforia(string token, string uuid)
    {
        string authorizationHeader = $"Bearer {token}";
        //string url = $"{vuforiaEndpoint}/eb25d27bece14a1081e79acc5b2a643d/dataset";
        string url = $"{vuforiaEndpoint}/{uuid}/dataset";

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", authorizationHeader);

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            byte[] results = request.downloadHandler.data;

            // Save the downloaded data to a file
            string wherePath = Path.Combine(Application.persistentDataPath + "/Zips/dataset.zip");
            File.WriteAllBytes(wherePath, results);
            ZipController.UnzipStarter();

            Debug.Log("Dataset downloaded successfully.");
        }
        else
        {
            Debug.LogError("Download failed. Error: " + request.error);
            //ZipController.UnzipStarter();
        }


        //yield return null;
    }

    public IEnumerator DeleteDatasetOnVuforia(string token, string uuid)
    {
        string authorizationHeader = $"Bearer {token}";
        string url = $"{vuforiaEndpoint}/{uuid}";
        Debug.Log(url);

        UnityWebRequest request = UnityWebRequest.Delete(url);

        request.SetRequestHeader("Authorization", authorizationHeader);
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.LogError("Delete request failed. Error: " + request.error);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Delete method successful");
            //byte[] results = request.downloadHandler.data;
            //string responseText = Encoding.UTF8.GetString(results);
            //Debug.Log("Response: " + responseText);
        }
        // yield return null;
    }





    private List<string> ObtainTokenAndUuid()
    {
        List<string> AuUiid = new();
        if (PlayerPrefs.HasKey(token))
        {
            string accessTokenBefore = PlayerPrefs.GetString(token);
            AccessTokenData tokenData = JsonUtility.FromJson<AccessTokenData>(accessTokenBefore);
            string accessToken = tokenData.access_token;
            //Debug.Log("Access Token: " + accessToken);
            AuUiid.Add(accessToken);
            if (PlayerPrefs.HasKey(uuid))
            {
                string vuforiaUuidPreparsed = PlayerPrefs.GetString(uuid);
                VuforiaUuid ParsedUuid = JsonUtility.FromJson<VuforiaUuid>(vuforiaUuidPreparsed);
                string parsedUuid = ParsedUuid.uuid;
                Debug.Log("VuforiaUuid: " + parsedUuid);
                AuUiid.Add(parsedUuid);
            }
            else
            {
                Debug.Log("No VuforiaUuid...");
            }
        }
        else
        {
            Debug.LogError("Access Token not found in cache. Make sure to obtain the token first.");

        }
        return AuUiid;
    }
}
