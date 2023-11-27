#pragma warning disable IDE0058 
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public class ZipController : MonoBehaviour
{
    private static ZipController instance;
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(StartZipProcess());
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(StartUnzipProcess());
        }
    }
    private void Awake()
    {
        instance = this;
    }
    public static void UnzipStarter()
    {
        instance.StartCoroutine(instance.StartUnzipProcess());
    }

    public void ZipStarter()
    {
        StartCoroutine(StartZipProcess());
    }
    public IEnumerator StartZipProcess()
    {
        yield return StartCoroutine(ZipIt(OnZipComplete));
    }
    public IEnumerator StartUnzipProcess()
    {
        yield return StartCoroutine(UnZipIt(OnUnzipComplete));
    }

    public IEnumerator ZipIt(Action<string> onComplete)
    {
        string zipPath = Path.Combine(Application.dataPath, "Prefabs", "Models");
        string wherePath = Path.Combine(Application.dataPath, "Prefabs", "Zips", "model.zip");

        ZipFile.CreateFromDirectory(zipPath, wherePath);

        yield return null;

        onComplete?.Invoke(wherePath);
    }

    public IEnumerator UnZipIt(Action onComplete)
    {
        string zipPath = Path.Combine(Application.dataPath, "StreamingAssets", "Vuforia");
        string wherePath = Path.Combine(Application.dataPath, "Prefabs", "Zips", "dataset.zip");

        ZipFile.ExtractToDirectory(wherePath, zipPath, true);

        yield return null;

        onComplete?.Invoke();
    }

    private void OnZipComplete(string filePath)
    {
        Debug.Log("Zip file created at: " + filePath);
    }

    private void OnUnzipComplete()
    {
        Debug.Log("Unzipping completed successfully");
    }
}
