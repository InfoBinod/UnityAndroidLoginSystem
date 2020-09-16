using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wrapper : MonoBehaviour
{
    // bool isPaused = false;
    //bool pauseStatus;
    // Start is called before the first frame update
    
    void Start()
    {
        string str = PlayerPrefs.GetString("Name", null);

        if (string.IsNullOrEmpty(str) != true)
        {
            
           // OnApplicationPause(pauseStatus);
            SceneManager.LoadScene(3);

        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
   // void OnApplicationPause(bool pause)
    //{
      //  isPaused = pause;
    //}
    // Update is called once per frame

}
