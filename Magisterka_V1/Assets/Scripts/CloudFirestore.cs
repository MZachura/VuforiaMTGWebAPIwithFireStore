#pragma warning disable IDE0058 // Expression value is never used
using UnityEngine;
using Firebase.Storage;
using Firebase.Extensions;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections;

public class CloudFirestore : MonoBehaviour
{
    private static CloudFirestore instance;
    FirebaseStorage storage;
    StorageReference storageReference;
    public Button Button1;
    public object signedUrl;
    private const string firebaseUrl = "FirebaseUrl";

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        storage = FirebaseStorage.DefaultInstance;
        //Button1.onClick.AddListener(Savedata);
        storageReference = storage.GetReferenceFromUrl("gs://mgrmiddleman.appspot.com/");
    }
    public static void SavingStarter(string path)
    {
        instance.StartCoroutine(instance.StartSaving(path));
    }
    public IEnumerator StartSaving(string path)
    {
        yield return StartCoroutine(SavedataToFirestore(OnSaveComplete, path));
    }
    public IEnumerator SavedataToFirestore(Action onComplete, string path)
    {

        //string filePath = Path.Combine(Application.dataPath, "prefabs", "models", "Assemblyv3.stl");
        byte[] fileBytes = File.ReadAllBytes(path);
        StorageReference uploadRef = storageReference.Child("uploads/NewTestCad.stl");

        uploadRef.PutBytesAsync(fileBytes).ContinueWithOnMainThread((task) =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());

            }
            else
            {
                Debug.Log("File uploaded");
                signedUrl = uploadRef.GetDownloadUrlAsync().ContinueWithOnMainThread((task) =>
                {
                    if (task.IsCanceled || task.IsFaulted)
                    {
                        Debug.Log("meh");
                    }
                    else
                    {
                        //Debug.Log(task.Result.ToString());
                        PlayerPrefs.SetString(firebaseUrl, task.Result.ToString());
                        PlayerPrefs.Save();
                        onComplete?.Invoke();
                    }
                });
            }
        });
        yield return null;
    }
    private void OnSaveComplete()
    {
        ModelTargetUploader.StepTwo();
    }

}
