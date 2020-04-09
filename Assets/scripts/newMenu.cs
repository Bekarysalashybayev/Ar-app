using UnityEngine;
using UnityEngine.SceneManagement;

public class newMenu : MonoBehaviour
{
    void Start()
    {
       /* menu.onClick.AddListener(()=> { SceneManager.LoadScene("Menu");  });
        Scan.onClick.AddListener(() => { SceneManager.LoadScene("Scan");  });
        profile.onClick.AddListener(() => {

            if (PlayerPrefs.GetString("userName") != "" && PlayerPrefs.GetString("password") != "")
            {
                Debug.Log(PlayerPrefs.GetString("userName"));
                SceneManager.LoadScene("Profile");
            }
            else
            {
                SceneManager.LoadScene("Login");
            }
        });*/
    }

    private void Update()
    {
        
    }

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
}
