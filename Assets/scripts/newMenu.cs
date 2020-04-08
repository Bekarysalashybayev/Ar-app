using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class newMenu : MonoBehaviour
{

    

    public void Login()
    {
        
        if (PlayerPrefs.GetString("userName") != "" && PlayerPrefs.GetString("password") != "")
        {
            Debug.Log(PlayerPrefs.GetString("userName"));
            SceneManager.LoadScene("Profile");
        }
        else
        {
            SceneManager.LoadScene("Login");
        }
    }
    public void TheoryMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void RegMenu()
    {
        SceneManager.LoadScene("RegisterMenu");
    }

    public void Register()
    {
        SceneManager.LoadScene("Register");
    }

    public void  scan()
    {
        if (PlayerPrefs.GetString("userName") != "" && PlayerPrefs.GetString("password") != "")
        {
            SceneManager.LoadScene("Scan");
        }
        else
        {
            /* if (UnityEditor.EditorUtility.DisplayDialog("AR-Geometry", "You need Log in", "Login", "Exit"))
             {
                 SceneManager.LoadScene(6);
             }
             else
             {
                 Application.Quit();
             }*/
        }
    }
    void showMessaeg()
    {
        
    }

    public void Theory()
    {
        SceneManager.LoadScene("NewTheory");
    }
    public void Testing()
    {
        SceneManager.LoadScene("TestContent");
    }
}
