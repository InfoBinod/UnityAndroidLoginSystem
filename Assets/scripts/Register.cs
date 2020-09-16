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

public class Register : MonoBehaviour
{
    private string conn, sqlQuery ;
    IDbConnection dbconn;
    IDbCommand dbcmd ;
    private IDataReader reader;
    public InputField username, password, email;
    public Text error;
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
        create_DB();
        insert_button();
        if (!File.Exists(filepath))

        {
            StartCoroutine(GetRequest());
            conn = "URI=file:" + filepath;

            Debug.Log("Stablishing connection to: " + conn);
            dbconn = new SqliteConnection(conn);
            dbconn.Open();
            create_DB();
            insert_button();

            // if it doesn't ->

            // open StreamingAssets directory and load the db ->
        }
    }
    public void create_DB()
    {
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open(); //Open connection to the database.
            dbcmd = dbconn.CreateCommand();

            sqlQuery = string.Format("CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY, username VARCHAR[50],password VARCHAR[50],email VARCHAR[50] )");
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();
            dbconn.Close();
        }
    }
    //Insert
    public void insert_button()
    {
       string pattern = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";

        if (username.text == "")
        {
            error.text= ("username ia NULL");
        }
        else if (password.text == "")
        {
            error.text = ("password is null");
        }
        else if (email.text == "")
        {
            error.text = ("email is null");
        }
        else if (username.text.Length < 6)
        {
            error.text = "username length must be 6 character";
        }
        else if (password.text.Length < 6)
        {
            error.text = ("password length must be 6 character");
        }
        
        else if (!Regex.IsMatch(email.text, pattern))
        {
            error.text = ("provide a valid email");
        }
        else
        {
            insert_function(username.text, password.text, email.text);
        }
    }
   
    

 

    //Insert To Database
    private void insert_function(string name, string password ,string email)
    {
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open(); //Open connection to the database.
            dbcmd = dbconn.CreateCommand();
          
            sqlQuery = string.Format("insert into users (username, password ,email) values (\"{0}\",\"{1}\",\"{2}\")", name, password, email);// table name
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();
            dbconn.Close();
        }
        //data_staff.text = "";
        Debug.Log("Insert Done  ");
       // PlayerPrefs.SetString("Name",username.text);
        SceneManager.LoadScene(1);
        reader_function();
    }
    //Read All Data For To Database
    private void reader_function()
    {
        // int idreaders ;
        string Namereaders, Passwordreaders ,EmailReaders;
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT  username , password , email " + "FROM users";// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                // idreaders = reader.GetString(1);
                Namereaders = reader.GetString(0);
                Passwordreaders = reader.GetString(1);
                EmailReaders = reader.GetString(2);
                // data_staff.text += Namereaders + Addressreaders + "\n";
                Debug.Log(" name =" + Namereaders + "Password=" + Passwordreaders + "email="  +EmailReaders);
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

void Update()
    {

    }
}