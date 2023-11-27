using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Buffers.Text;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using System.Net;

public class ScreenshotDisplayController : MonoBehaviour
{
    public string screenshotPath;
    public GameObject ScreenShotCanvas;
    public TMP_Text label;
    public Image screenshotImage;
    public Text AssemblyID;
    private string conn;

    public GameObject prefabButton;
    public RectTransform ParentPanel;
    public RectTransform ParentPanel2;
    public GameObject Canvas;
    public GameObject ScrollPanel;
    public GameObject ScrollPanel2;
    public ColorBlock colors;
    public GameObject Panel;
    public GameObject Panel2;
    public GameObject Base1;
    public GameObject Base2;
    public GameObject Base3;

    public Button Back1;
    public Button Back2;
    public Button Back3;

    private string dbName = "MGRDatabase.s3db";

    // Start is called before the first frame update
    void Awake()
    {
        dbName = "MGRDatabase.s3db";
        string filepath = Application.persistentDataPath + "/" + dbName;

        if (!File.Exists(filepath))
        {

            // UNITY_ANDROID
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/MGRDatabase.s3db");
            //WWW loadDB = new WWW("file://" + Application.persistentDataPath + "/MGRDatabase.s3db");
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDB.bytes);
        }
        //conn = "URI=file:" + filepath;
        conn = "URI=file:" + filepath;
    }
    private void Start()
    {
        DisplayAssembly();
        Base1.SetActive(true);
        Base2.SetActive(false);
        Base3.SetActive(false);
        Back2.onClick.AddListener(OnBack2Click);
        Back3.onClick.AddListener(OnBack3Click);
    }
    private void OnBack2Click()
    {

        Back1.gameObject.SetActive(true);
        Back2.gameObject.SetActive(false);
        Base1.SetActive(true);
        Base2.SetActive(false);
        for (var i = ScrollPanel2.transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(ScrollPanel2.transform.GetChild(i).gameObject);
        }
        //destroyChildren
    }
    private void OnBack3Click()
    {
        Back2.gameObject.SetActive(true);
        Back3.gameObject.SetActive(false);
        Base2.SetActive(true);
        Base3.SetActive(false);
    }
    public void DisplayAssembly()
    {
        //select * from assembly
        string filepath = Application.persistentDataPath + "/" + dbName;

        if (!File.Exists(filepath))
        {

            // UNITY_ANDROID
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/MGRDatabase.s3db");
            //WWW loadDB = new WWW("file://" + Application.persistentDataPath + "/MGRDatabase.s3db");
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDB.bytes);
        }
        //conn = "URI=file:" + filepath;
        conn = "URI=file:" + filepath;
        using (IDbConnection connection = new SqliteConnection(conn))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Assembly;";
                //command.ExecuteNonQuery();
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!bool.Parse(reader["AssemblyStatus"].ToString()))
                        {

                            GameObject newButton = (GameObject)Instantiate(prefabButton);
                            newButton.transform.SetParent(ParentPanel, false);
                            newButton.transform.localScale = new Vector3(1, 1, 1);

                            Button tempButton = newButton.GetComponent<Button>();
                            label = tempButton.transform.Find("Text").GetComponent<TMP_Text>();
                            tempButton.GetComponent<Image>().color = Color.green;
                            label.text = reader["ID"].ToString();
                            int id = int.Parse(reader["ID"].ToString());
                            tempButton.onClick.AddListener(() => Button2Clicked());
                        }
                        else if (bool.Parse(reader["AssemblyStatus"].ToString()))
                        {
                            Debug.Log("yis");

                            GameObject newButton = (GameObject)Instantiate(prefabButton);
                            newButton.transform.SetParent(ParentPanel, false);
                            newButton.transform.localScale = Vector3.one;
                            Button tempButton = newButton.GetComponent<Button>();
                            label = tempButton.transform.Find("Text").GetComponent<TMP_Text>();
                            tempButton.GetComponent<Image>().color = Color.red;
                            label.text = reader["ID"].ToString();
                            int id = int.Parse(reader["ID"].ToString());
                            tempButton.onClick.AddListener(() => ButtonClicked(id));
                        }

                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
    }
    private int Button2Clicked()
    {
        return 0;
    }
    private void ButtonClicked(int id)
    {
        if (Panel != null)
        {

            Base1.SetActive(false);
            Back1.gameObject.SetActive(false);
            Back2.gameObject.SetActive(true);
            Base2.SetActive(true);
            if (Panel.activeSelf)
            { 
                SetButtons(id);
            }
        }
    }




    public void SetButtons(int id)
    {
        string filepath = Application.persistentDataPath + "/" + dbName;
        conn = "URI=file:" + filepath;
        using (IDbConnection connection = new SqliteConnection(conn))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT LocalID,Note FROM AssemblyFaults WHERE ID = {id};";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        GameObject newButton = (GameObject)Instantiate(prefabButton);
                        newButton.transform.SetParent(ParentPanel2, false);
                        newButton.transform.localScale = new Vector3(1, 1, 1);
                        Button tempButton = newButton.GetComponent<Button>();
                        label = tempButton.transform.Find("Text").GetComponent<TMP_Text>();
                        tempButton.GetComponent<Image>().color = Color.green;
                        label.text = reader["Note"].ToString();
                        int Lid = int.Parse(reader["LocalID"].ToString());
                        tempButton.onClick.AddListener(() => Button3Clicked(Lid));

                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
        // wyœwietl notatkê
    }
    private void Button3Clicked(int id)
    {
        if (Panel2 != null)
        {

            Base2.SetActive(false);
            Back2.gameObject.SetActive(false);
            Back3.gameObject.SetActive(true);
            Base3.SetActive(true);
            if (Panel2.activeSelf)
            {

                SetScreenshot(id);
            }
        }
    }

    public void SetScreenshot(int id)
    {
        string filepath = Application.persistentDataPath + "/" + dbName;

        if (!File.Exists(filepath))
        {

            // UNITY_ANDROID
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/MGRDatabase.s3db");
            //WWW loadDB = new WWW("file://" + Application.persistentDataPath + "/MGRDatabase.s3db");
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDB.bytes);
        }
        //conn = "URI=file:" + filepath;
        conn = "URI=file:" + filepath;
        byte[] imageBytes;
        using (IDbConnection connection = new SqliteConnection(conn))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT Screenshot FROM AssemblyFaults WHERE LocalID = {id};";
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        imageBytes = (byte[])reader["Screenshot"];
                        DisplayScreenshot(imageBytes);
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
    }

    public void DisplayFaultNotes(int id)
    {
        string filepath = Application.persistentDataPath + "/" + dbName;

        if (!File.Exists(filepath))
        {

            // UNITY_ANDROID
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/MGRDatabase.s3db");
            //WWW loadDB = new WWW("file://" + Application.persistentDataPath + "/MGRDatabase.s3db");
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDB.bytes);
        }
        //conn = "URI=file:" + filepath;
        conn = "URI=file:" + filepath;
        using (IDbConnection connection = new SqliteConnection(conn))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT (ID,Note) FROM AssemblyFaults where ID ={id};";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    //Postaw nowy panel
    //Wy³¹cz panel scrollowany
    // przy klikniêciu w przycisk nowy panel 
    // W koñcu screenshot!!! 
    //Przyciski Back z notatek
    //Przyciski Back ze screenshota


    public void DisplayScreenshot(byte[] imageBytes)
    {

        if (imageBytes != null)
        {
            Texture2D tex = new Texture2D(1, 1);
            if (tex != null)
            {
                tex.LoadImage(imageBytes);
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
                screenshotImage.sprite = sprite;
            }
        }
        //ARUICanvas.gameObject.SetActive(false);
        //ScreenshotCanvas.gameObject.SetActive(true);
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
}
