using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.IO;


public class ARUI : MonoBehaviour
{
    public Button BackButton;
    public Button EndButton;
    public Button AcceptButton;
    public Button DeclineButton;
    public Button CaptureButton;
    public Button Accept1Button;
    public Button Decline1Button;
    public Button Accept2Button;
    public Button Decline2Button;

    public Button Accept3Button;
    public Button Decline3Button;
    public Button End2Button;


    public Button Accept4Button;
    public Button Decline4Button;
    public Button End3Button;


    public Canvas StartCanvas;
    public Canvas ARUICanvas;
    public Canvas ScreenshotCanvas;
    public Canvas WhatsWrongCanvas;
    public Canvas AcceptCorruptionPanel;
    public Canvas AcceptPanelOfCorrectAssembly;

    public GameObject Placeholder;
    public GameObject Target;

    public Image screenshotImage;
    private string screenshotPath = "screenshot.png";
    //private string screenshotPath = Application.persistentDataPath + "/screenshot.png";
    public TMP_Text IDText;
    public TMP_InputField IDInput;
    public TMP_InputField WhatsWrongNote;
    public GameObject Pen;

    DatabaseScript databaseScript = new DatabaseScript();
    private int isCorrupted;
    private void Start()
    {
        Placeholder.SetActive(false);
        Target.SetActive(true);
        StartCanvas.gameObject.SetActive(true);
        ARUICanvas.gameObject.SetActive(false);
        ScreenshotCanvas.gameObject.SetActive(false);
        WhatsWrongCanvas.gameObject.SetActive(false);
        AcceptCorruptionPanel.gameObject.SetActive(false);
        AcceptPanelOfCorrectAssembly.gameObject.SetActive(false);

        IDInput.keyboardType = TouchScreenKeyboardType.NumberPad;

        EndButton.onClick.AddListener(OnEndClick);

        BackButton.onClick.AddListener(OnBackClick);
        AcceptButton.onClick.AddListener(OnAcceptClick);
        DeclineButton.onClick.AddListener(OnDeclineClick);
        CaptureButton.onClick.AddListener(OnCaptureClick);

        Accept1Button.onClick.AddListener(OnAccept1Click);
        Decline1Button.onClick.AddListener(OnDecline1Click);

        Accept2Button.onClick.AddListener(OnAccept2Click);
        Decline2Button.onClick.AddListener(OnDecline2Click);

        Accept3Button.onClick.AddListener(OnAccept3Click);
        Decline3Button.onClick.AddListener(OnDecline3Click);
        End2Button.onClick.AddListener(OnEnd2Click);

        Accept4Button.onClick.AddListener(OnAccept4Click);
        Decline4Button.onClick.AddListener(OnDecline4Click);
        End3Button.onClick.AddListener(OnEnd3Click);

        isCorrupted = 0;
    }


    //Buttons

    private void OnBackClick()
    {
        SceneManager.LoadScene("StartMenu");
    }
    private void OnAcceptClick()
    {
        StartCanvas.gameObject.SetActive(false);
        ARUICanvas.gameObject.SetActive(true);
        IDText.text = IDInput.text;
        DeleteFile();
    }
    private void OnDeclineClick()
    {
        SceneManager.LoadScene("StartMenu");

    }
    private void OnEndClick()
    {
        if (isCorrupted > 0)
        {
            AcceptCorruptionPanel.gameObject.SetActive(true);
        }
        else
        {
            AcceptPanelOfCorrectAssembly.gameObject.SetActive(true);
        }//done

    }

    private void OnCaptureClick()
    {
        //zrób screena i wyślij na Image UI
        StartCoroutine(TakeAScreenshot());
        //done
    }

    private void OnAccept1Click()
    {
        ScreenshotCanvas.gameObject.SetActive(false);
        WhatsWrongCanvas.gameObject.SetActive(true);
        //done
    }
    private void OnDecline1Click()
    {
        ScreenshotCanvas.gameObject.SetActive(false);
        ARUICanvas.gameObject.SetActive(true);
        DeleteFile();
        ClearImageTexture();
        ClearDrawing();
        //done
    }
    private void OnAccept2Click()
    {
        WhatsWrongCanvas.gameObject.SetActive(false);
        ARUICanvas.gameObject.SetActive(true);
        ClearImageTexture();
        ClearDrawing();
        string note = WhatsWrongNote.text.ToString();
        int idText = int.Parse(IDText.text);

        databaseScript.AddToAssemblyFaults(idText, note, Application.persistentDataPath + "/" + screenshotPath);
        isCorrupted++;
        //done

    }

    private void OnDecline2Click()
    {
        ScreenshotCanvas.gameObject.SetActive(true);
        WhatsWrongCanvas.gameObject.SetActive(false);
    }

    private void OnAccept3Click()
    {
        int idText = int.Parse(IDText.text);
        bool i = true;
        databaseScript.InsertGoodOrBad(i, idText);
        SceneManager.LoadScene("StartMenu");
        //done
    }

    private void OnDecline3Click()
    {
        AcceptPanelOfCorrectAssembly.gameObject.SetActive(false);
        ARUICanvas.gameObject.SetActive(true);
    }

    private void OnAccept4Click()
    {
        //done
        int idText = int.Parse(IDText.text);
        bool i = false;
        databaseScript.InsertGoodOrBad(i, idText);
        SceneManager.LoadScene("StartMenu");
    }

    private void OnDecline4Click()
    {
        AcceptCorruptionPanel.gameObject.SetActive(false);
        ARUICanvas.gameObject.SetActive(true);
    }

    private void OnEnd2Click()
    {
        SceneManager.LoadScene("StartMenu");
        int idText = int.Parse(IDText.text);
        databaseScript.DeleteFormAssemblyDatabase(idText);
        //done
    }
    private void OnEnd3Click()
    {
        SceneManager.LoadScene("StartMenu");
        int idText = int.Parse(IDText.text);
        databaseScript.DeleteFormAssemblyDatabase(idText);
        //done
    }



    //Methods 


    private IEnumerator TakeAScreenshot()
    {
        ScreenCapture.CaptureScreenshot(screenshotPath);

        // Wait for a frame to ensure the screenshot capture process has started
        yield return new WaitForEndOfFrame();

        // Continue waiting until the file exists (screenshot is saved)
        while (!System.IO.File.Exists(Application.persistentDataPath + "/" + screenshotPath))
        {
            yield return null;
        }

        // Display the captured screenshot
        DisplayScreenshot();
    }
    public void DisplayScreenshot()
    {
        if (screenshotImage != null)
        {
            Texture2D tex = LoadTextureFromFile(Application.persistentDataPath + "/" + screenshotPath);
            if (tex != null)
            {
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
                screenshotImage.sprite = sprite;
            }
        }
        ARUICanvas.gameObject.SetActive(false);
        ScreenshotCanvas.gameObject.SetActive(true);
    }
    private Texture2D LoadTextureFromFile(string path)
    {
        byte[] fileData = System.IO.File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(fileData); // Load the image data
        return tex;
    }
    public void DeleteFile()
    {
        if (File.Exists(Application.persistentDataPath + "/" + screenshotPath))
        {
            File.Delete(Application.persistentDataPath + "/" + screenshotPath);
            Debug.Log("File deleted: " + screenshotPath);
        }
        else
        {
            Debug.LogWarning("File does not exist: " + screenshotPath);
        }
    }

    public void ClearImageTexture()
    {
        if (screenshotImage != null)
        {
            screenshotImage.sprite = null;
        }
        else
        {
            Debug.LogWarning("Image component not assigned!");
        }
    }

    public void ClearDrawing()
    {
        if (Pen != null)
        {
            int childCount = Pen.transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = Pen.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Parent object is not assigned!");
        }
    }
}


