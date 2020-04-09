using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class Progress : MonoBehaviour
{
    public Text txtTotal;
    public Text txtEasy;
    public Text txtMedium;
    public Text txtHard;

    public int easy = 0;
    public int medium = 0;
    public int hard = 0;

    public int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetScore("1"));
        StartCoroutine(GetScore("2"));
        StartCoroutine(GetScore("3"));
    }

    // Update is called once per frame
    void Update()
    {
        if (this.count == 3)
        {
            txtTotal.text = easy + medium + hard + " points";
            txtEasy.text = easy + " pts";
            txtMedium.text = medium + " pts";
            txtHard.text = hard + " pts";
            this.count++;
        }
    }
    public void Easy()
    {
        progressAll.levelid = "1";
        SceneManager.LoadScene("ProgressPage");
    }
    public void Medium()
    {
        progressAll.levelid = "2";
        SceneManager.LoadScene("ProgressPage");
    }
    public void Hard()
    {
        progressAll.levelid = "3";
        SceneManager.LoadScene("ProgressPage");
    }
    public IEnumerator GetScore(string levelid)
    {

        WWWForm form = new WWWForm();
        form.AddField("id", PlayerPrefs.GetString("id"));
        form.AddField("levelid", levelid);

        using (UnityWebRequest www = UnityWebRequest.Post("http://augmented.zzz.com.ua/ar/getScoreByLevel.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                //Debug.Log(www.error);
            }
            else
            {
                if (!www.downloadHandler.text.Contains("div"))
                {
                    JSONArray jsonArray = JSON.Parse(www.downloadHandler.text) as JSONArray;
                    if (levelid == "1")
                    {
                        this.easy = jsonArray[0].AsObject["score"];
                    }
                    else if (levelid == "2")
                    {
                        this.medium = jsonArray[0].AsObject["score"];
                    }
                    else if (levelid == "3")
                    {
                        this.hard = jsonArray[0].AsObject["score"];
                    }
                    
                }
                this.count++;
            }
        }
    }
}
