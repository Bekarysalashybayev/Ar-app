using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System;

public class Profile : MonoBehaviour
{
    public Text user;
    void Start()
    {
        //StartCoroutine(loginDB(PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("password")));
        user.text = PlayerPrefs.GetString("userName");
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogOut()
    {
        PlayerPrefs.SetString("userName", "");
        PlayerPrefs.SetString("password", "");
        SceneManager.LoadScene(6);
    }

    IEnumerator loginDB(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("https://bekarysalashybaev.000webhostapp.com/arrapp/getDate.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text.Equals("UserName does not exists"))
                {
                    SceneManager.LoadScene(6);
                }

                else
                {
                    if (www.downloadHandler.text.Equals("Wrong Credentials"))
                    {
                        SceneManager.LoadScene(6);
                    }
                    else
                    {
                        SceneManager.LoadScene(4);
                    }

                }
            }
        }
    }
}
