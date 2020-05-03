using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using static ContentTheory;



public class Profile : MonoBehaviour
{
    public Text userName;
    public Text score;

    void Start()
    {
        // user.text = PlayerPrefs.GetString("userName");
        UpdateItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateItems()
    {
        if (PlayerPrefs.GetString("user") == "")
        {
            string url = "http://augmented.zzz.com.ua/ar/getUser.php";
            WWWForm form = new WWWForm();
            form.AddField("email", PlayerPrefs.GetString("userName"));
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            StartCoroutine(GetUser(www));
        }
        else
        {
            OnReceivedModels();
        }
        
    }

    void OnReceivedModels()
    {
        userName.text = PlayerPrefs.GetString("user");
        score.text = "Scores: " + 37;
    }

    public void LogOut()
    {
        PlayerPrefs.SetString("userName", "");
        PlayerPrefs.SetString("password", "");
        PlayerPrefs.SetString("user", "");
        PlayerPrefs.SetString("id", "");
        PlayerPrefs.SetString("score", "");
        SceneManager.LoadScene(6);
    }

    public void progressPage()
    {
        SceneManager.LoadScene("Progress");
    }

    IEnumerator GetUser(UnityWebRequest www)
    {
        yield return www.SendWebRequest();


        if (www.isNetworkError || www.isHttpError)
        {
           // Debug.Log(www.error);
            SceneManager.LoadScene(6);
        }
        else
        {
            if (www.downloadHandler.text.Contains("div"))
            {
                //Debug.Log(www.downloadHandler.text);
                SceneManager.LoadScene(6);
            }
            else
            {
                User[] users = JsonHelper.getJsonArray<User>(www.downloadHandler.text);
                userName.text = users[0].FirstName + " " + users[0].LastName;
                PlayerPrefs.SetString("user", users[0].FirstName + " " + users[0].LastName);
                PlayerPrefs.SetString("id", users[0].id);
                //Debug.Log(www.downloadHandler.text);
                //Debug.Log(PlayerPrefs.GetString("user"));
            }
        }
    }



    [System.Serializable]
    public class User
    {
        public string id;
        public string FirstName;
        public string LastName;
        public string Score;
    }
}
