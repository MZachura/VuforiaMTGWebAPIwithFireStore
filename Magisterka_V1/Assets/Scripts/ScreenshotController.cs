using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotController : MonoBehaviour
{
    public Button CaptureButton;
    public Image CapturedImage;
    private void Start()
    {
        CaptureButton.onClick.AddListener(onClicked);
    }
    private void onClicked()
    {
        StartCoroutine(TakeScreenShot());
    }

    IEnumerator TakeScreenShot()
    {
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot("screenshot.png");
    }
}
