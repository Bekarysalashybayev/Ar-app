using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System;


public class User
{
   public static string userName;
   public static string password;
}

public class NewLogin : MonoBehaviour
{

    public InputField userName;
    public InputField password;

    public Text errorUserName;
    public Text errorPaswword;

    public bool emailIsExist = false;
    void Start()
    {

    }


    public void Login()
    {
        StartCoroutine(loginDB(userName.text, password.text));
        
    }

    IEnumerator  loginDB(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://augmented.zzz.com.ua/ar/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text.Contains("UserName does not exists"))
                {
                    errorUserName.text = "UserName is Error";
                }
                
                else
                {
                    if (www.downloadHandler.text.Contains("Password incorrect"))
                    {
                        errorUserName.text = "";
                        errorPaswword.text = "Password is Error";
                    }
                    else
                    {
                        errorPaswword.text = "";
                        PlayerPrefs.SetString("user", "");
                        PlayerPrefs.SetString("userName", username);
                        PlayerPrefs.SetString("password", password);

                        SceneManager.LoadScene("Profile");
                    }
                    
                }
            }
        }
    }
}
