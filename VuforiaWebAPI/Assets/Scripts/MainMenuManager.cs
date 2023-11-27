using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class MainMenuManager : MonoBehaviour
{
    public Button ArControl;
    public Button CreateNewAssembly;
    public Button PreviousAssemblies;
    public Button ExitApp;
    public Button BackButton;
    public Button BackButton1;
    public Canvas MenuCanvas;
    public Canvas PrevCanvas;
    public Canvas DatasetCreationCanvas;

    public GameObject Canvas;
    public GameObject ScrollPanel;
    public GameObject Panel;
    public GameObject prefabButton;
    public RectTransform ParentPanel;


    void Start()
    {
        MenuCanvas.gameObject.SetActive(true);
        PrevCanvas.gameObject.SetActive(false);
        DatasetCreationCanvas.gameObject.SetActive(false);
        ArControl.onClick.AddListener(OnArContolClick);
        CreateNewAssembly.onClick.AddListener(OnCreateClick);
        PreviousAssemblies.onClick.AddListener(OnPreviousClick);
        ExitApp.onClick.AddListener(OnExitClick);
        BackButton.onClick.AddListener(OnBackClick);
        BackButton1.onClick.AddListener(OnBackClick1);

    }
    private void OnArContolClick()
    {
        SceneManager.LoadScene("ARControllScene");
    }
    private void OnCreateClick()
    {
        MenuCanvas.gameObject.SetActive(false);
        DatasetCreationCanvas.gameObject.SetActive(true);
    }
    private void OnPreviousClick()
    {
        MenuCanvas.gameObject.SetActive(false);
        PrevCanvas.gameObject.SetActive(true);
    }
    private void OnExitClick()
    {
        Application.Quit();
    }
    private void OnBackClick()
    {
        DatasetCreationCanvas.gameObject.SetActive(false);
        MenuCanvas.gameObject.SetActive(true);
    }
    private void OnBackClick1()
    {
        PrevCanvas.gameObject.SetActive(false);
        MenuCanvas.gameObject.SetActive(true);
    }


}
