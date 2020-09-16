using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//References
using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    private IDataReader reader;
    public InputField username, password;
    public Text error;
    //public static bool isLoggedIn;
    //public Text data_staff;

    string DatabaseName = "roomDecor.s3db";
    void start()
    {
        allfunctions();
    }

    public void allfunctions()
    {

        // string filepath = Application.dataPath + "/Plugins/" + DatabaseName;
        string filepath = Application.persistentDataPath + "/" + DatabaseName;
        conn = "URI=file:" + filepath;

        Debug.Log("Stablishing connection to: " + conn);
        dbconn = new SqliteConnection(conn);
        dbconn.Open();
        insert_button();
        if (!File.Exists(filepath))

        {
            StartCoroutine(GetRequest());
            conn = "URI=file:" + filepath;

            Debug.Log("Stablishing connection to: " + conn);
            dbconn = new SqliteConnection(conn);
            dbconn.Open();
            insert_button();

            // if it doesn't ->

            // open StreamingAssets directory and load the db ->
        }
    }

    //Insert
    public void insert_button()
    {


        if (username.text == "")
        {
            error.text = ("username ia NULL");
        }
        else if (password.text == "")
        {
            error.text = ("password is null");
        }
        else
        {
            reader_function();
        }
    }
    //Insert To Database
    
    
    //Read All Data For To Database
    private void reader_function()
    {
        // int idreaders ;
        string Namereaders, Passwordreaders;
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT  username , password  " + "FROM users";// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                // idreaders = reader.GetString(1);
                Namereaders = reader.GetString(0);
                Passwordreaders = reader.GetString(1);
                if(Namereaders != username.text && Passwordreaders != password.text)
                {
                    error.text = "invalid username and password";
                }
                else if (Namereaders == username.text && Passwordreaders != password.text)
                {
                    error.text = "invalid  password";
                }
                else if (Namereaders != username.text && Passwordreaders == password.text)
                {
                    error.text = "invalid username ";
                }
                else
                {
                    PlayerPrefs.SetString("Name", Namereaders);
                    SceneManager.LoadScene(3);
                    
                  //isLoggedIn = true;
                }

                // data_staff.text += Namereaders + Addressreaders + "\n";
                Debug.Log(" name =" + Namereaders + "Password=" + Passwordreaders);
            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            //       dbconn = null;

        }
    }
    IEnumerator GetRequest()
    {
        UnityWebRequest loadDB = new UnityWebRequest("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android

        while (!loadDB.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
        yield return loadDB.SendWebRequest();
    }
    // then save to Application.persistentDataPath

    
}