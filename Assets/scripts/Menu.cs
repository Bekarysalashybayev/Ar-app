using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject message;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Theory()
    {
        if (PlayerPrefs.GetString("userName") != "" && PlayerPrefs.GetString("password") != "")
        {
            SceneManager.LoadScene("NewTheory");
        }
        else
        {
            message.SetActive(true);

        }
    }
    public void Testing()
    {
        if (PlayerPrefs.GetString("userName") != "" && PlayerPrefs.GetString("password") != "")
        {
            SceneManager.LoadScene("TestContent");
        }
        else
        {
            message.SetActive(true);

        }
    }
    public void Progress()
    {
        if (PlayerPrefs.GetString("userName") != "" && PlayerPrefs.GetString("password") != "")
        {
            SceneManager.LoadScene("Progress");
        }
        else
        {
            message.SetActive(true);

        }
    }

    public void login()
    {
        message.SetActive(false);
        SceneManager.LoadScene("Login");
    }

    public void hide()
    {
        message.SetActive(false);
    }
}
