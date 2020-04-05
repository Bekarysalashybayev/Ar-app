using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System;

public class newMenu : MonoBehaviour
{
    public void Login()
    {
        
        if (PlayerPrefs.GetString("userName") != "" && PlayerPrefs.GetString("password") != "")
        {
            Debug.Log(PlayerPrefs.GetString("userName"));
            SceneManager.LoadScene(4);
        }
        else
        {
            SceneManager.LoadScene(6);
        }
    }
    public void Theory()
    {
        SceneManager.LoadScene(0);
    }

    public void RegMenu()
    {
        SceneManager.LoadScene(2);
    }

    public void Register()
    {
        SceneManager.LoadScene(3);
    }

    public void  scan()
    {
        SceneManager.LoadScene(5);
    }

    
}
