﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class TDetail : MonoBehaviour
{

    public Text tname;
    public Text def;
    public Text comment;
    public Text theorem;
    public Text evidence;
    public RawImage img;
    public RawImage img2;
    public static string id;
    public JSONArray jsonArray;

    public Button ck;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GeTheories("1"));
        ck.onClick.AddListener( ()=> { play(); });
    }

    void play()
    {
        SceneManager.LoadScene(0);
    }

    public IEnumerator GeTheories(string id)
    {

        WWWForm form = new WWWForm();
        form.AddField("id", id);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/get/getDate.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                this.jsonArray = JSON.Parse(www.downloadHandler.text) as JSONArray;
                if (id != null)
                {
                    tname.text = this.jsonArray[0].AsObject["name"];
                    def.text = getCorrectText(this.jsonArray[0].AsObject["definition"]);
                    comment.text = this.jsonArray[0].AsObject["comment"];
                    theorem.text = this.jsonArray[0].AsObject["theorem"];
                    evidence.text = this.jsonArray[0].AsObject["evidence"];
                    StartCoroutine(LoadImage(img, this.jsonArray[0].AsObject["img"]));
                    StartCoroutine(LoadImage(img2, this.jsonArray[0].AsObject["img2"]));
                }
            }
        }
    }

    private string getCorrectText(JSONNode jSONNode)
    {
        string name = jSONNode;
        name = name.Replace("kt", "<=");
        name = name.Replace('g', '*');
        return name;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator LoadImage(RawImage image, string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            image.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }

    }
}