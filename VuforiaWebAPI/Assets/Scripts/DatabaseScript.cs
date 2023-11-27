using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.UI;
using System.IO;
using TMPro;


public class DatabaseScript : MonoBehaviour
{
    public Text AssemblyID;
    private string conn;

    public GameObject prefabButton;
    public RectTransform ParentPanel;
    public GameObject Canvas;
    public GameObject ScrollPanel;
    public TMP_Text label;
    public ColorBlock colors;
    public GameObject Panel;
    public GameObject Base1;
    public GameObject Base2;

    public ScreenshotDisplayController screenshotScript;
    private string dbName = "MGRDatabase.s3db";

    // Start is called before the first frame update
    private void Awake()
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
        Debug.Log("filepath: " + filepath);
    }
    private void Start()
    {
        CreateDB();

    }


    public void CreateDB()
    {
        using (IDbConnection connection = new SqliteConnection(conn))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS Assembly (ID INTEGER ,AssemblyStatus BOOLEAN  NULL);";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS AssemblyFaults (LocalID INTEGER PRIMARY KEY,ID INTEGER NULL,Screenshot BLOB  NULL,Note TEXT  NULL);";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }


    public void InsertGoodOrBad(bool status, int id)
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
        int statusParsed = status ? 1 : 0;
        Debug.Log(statusParsed);
        using (IDbConnection connection = new SqliteConnection(conn))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO Assembly(ID, AssemblyStatus) VALUES ({id},{statusParsed});";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }


    }

    public void AddToAssemblyFaults(int id, string note, string imagepath)
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
        byte[] imageBytes = File.ReadAllBytes(imagepath);
        using (IDbConnection connection = new SqliteConnection(conn))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO AssemblyFaults(ID, Screenshot, Note) VALUES (@id, @imageBytes, @note);";

                // Define parameters for the query
                command.Parameters.Add(new SqliteParameter("@id", id));
                command.Parameters.Add(new SqliteParameter("@imageBytes", imageBytes));
                command.Parameters.Add(new SqliteParameter("@note", note));

                // Execute the query
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void DeleteFormAssemblyDatabase(int id)
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
                command.CommandText = $"DELETE * FROM AssemblyFaults WHERE ID = {id};";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }





    public void DiplayFaultScreenshots(int localID)
    {

        //select screenshot from assemblyfaults
        string filepath = Application.persistentDataPath + "/" + dbName;
        conn = "URI=file:" + filepath;
        using (IDbConnection connection = new SqliteConnection(conn))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT (Screenshot) FROM AssemblyFaults where ID ={localID};";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}
