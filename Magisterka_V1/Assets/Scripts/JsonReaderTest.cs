using System;
using System.IO;
using UnityEngine;



public class JsonReaderTest : MonoBehaviour
{
    [Serializable]
    private class MyJsonData
    {
        public string CLIENT_ID { get; set; }
        public string CLIENT_SECRET { get; set; }
    }

    public string jsonFilePath = "appSettings";
    // Start is called before the first frame update
    void Start()
    {
        LoadJsonData();
    }

    private void LoadJsonData()
    {
        if (File.Exists(jsonFilePath))
        {
            try
            {
                // Read the JSON file content
                TextAsset jsonContent = Resources.Load<TextAsset>(jsonFilePath);
                Debug.Log(jsonContent.text);
                MyJsonData jsonData = JsonUtility.FromJson<MyJsonData>(jsonContent.text);
                Debug.Log(jsonData);
                //Debug.Log(jsonData.CLIENT_ID);
                //Debug.Log(jsonData.CLIENT_ID);

                // Access the fields
                //string clientId = jsonData.CLIENT_ID;
                //string clientSecret = jsonData.CLIENT_SECRET;

                // Use the values as needed
                //Debug.Log("ClientCreds: " + clientId);
                //Debug.Log("ClientSecret: " + clientSecret);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("The JSON file does not exist.");
        }
    }
}


