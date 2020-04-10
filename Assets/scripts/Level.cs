using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SimpleJSON;
using System;

public class Level : MonoBehaviour
{
    public static string category;
    public static string theoryid;

    public int levelMedium = 0;
    public int levelHard = 0;

    public GameObject medium;
    public GameObject medium1;
    public GameObject hard;
    public GameObject hard1;

    public Text theoryName;

    int count = 1;

    void Start()
    {
        StartCoroutine(GetScore("1"));
        StartCoroutine(GetScore("2"));
        theoryName.text = category;

        Testing.category = category;
        Testing.theoryid = theoryid;


       // Debug.Log("id is " +PlayerPrefs.GetString("id") + " theory id is " +theoryid);
       // Debug.Log(levelMedium + "          " + levelHard);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.count == 1)
        {
            if (levelMedium > 6)
            {
                medium.SetActive(false);
                medium1.SetActive(true);
            }
            if (levelHard > 6)
            {
                hard.SetActive(false);
                hard1.SetActive(true);
            }
        }
        


    }

    public void playEasy()
    {
        Testing.level = "1";
        SceneManager.LoadScene("Testing");
    }
    public void playMedium()
    {

        if (levelMedium > 6)
        {
            Testing.level = "2";
            SceneManager.LoadScene("Testing");
        }
    }
    public void playHard()
    {
        if (levelHard > 6)
        {
            Testing.level = "3";
            SceneManager.LoadScene("Testing");
        }
    }

    public IEnumerator GetScore(string levelid)
    {

        WWWForm form = new WWWForm();
        form.AddField("levelid", levelid);
        form.AddField("userid", PlayerPrefs.GetString("id"));
        form.AddField("theoryid", theoryid);

        using (UnityWebRequest www = UnityWebRequest.Post("http://augmented.zzz.com.ua/ar/getScoreForLevel.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
               
                //Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text != "0" && www.downloadHandler.text.Length < 20)
                {
                    JSONArray jsonArray = JSON.Parse(www.downloadHandler.text) as JSONArray;
                   
                    int score = 0;
                    string str = jsonArray[0].AsObject["Score"];
                    String[] strlist = str.Split('"');
                    Int32.TryParse(strlist[0], out score);
                    //Debug.Log(score);
                    if (levelid == "1")
                     {
                         this.levelMedium = score;
                     }
                     else
                     {
                         this.levelHard = score;
                     }
                }
               
            }
        }
    }
}
