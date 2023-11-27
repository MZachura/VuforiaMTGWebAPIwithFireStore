using System.IO;
using System.Collections;
using UnityEngine;

public class FileUploader : MonoBehaviour
{

    public string FinalPath;
    public UnityEngine.UI.Button createDatasetButton;

    public static void LoadFile()
    {
        string FileType = NativeFilePicker.ConvertExtensionToFileType("stl");

        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                Debug.LogWarning("Operation cancelled");
            }
            else
            {
                //FinalPath = path;
                Debug.Log("Picked file");
                OnLoadedAndSavedFileComplete(path);
            }
        }, new string[] { FileType });
    }

    private static void OnLoadedAndSavedFileComplete(string path)
    {
        ModelTargetUploader.StepOne(path);
    }


}
