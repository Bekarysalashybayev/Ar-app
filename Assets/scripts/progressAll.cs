using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;
using static ContentTheory;

public class progressAll : MonoBehaviour
{
    public static string levelid;

    public GameObject preFab;
    public RectTransform content;

    ProgressItemModel[] models;

    public Text level;
    // Start is called before the first frame update
    void Start()
    {
        switch (levelid)
        {
            case "1":
                level.text = "Easy List";
                break;
            case "2":
                level.text = "Medium List";
                break;
            case "3":
                level.text = "Hard List";
                break;
        }

        UpdateItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateItems()
    {
        string url = "http://augmented.zzz.com.ua/ar/GetAllScoresByLevel.php";
        WWWForm form = new WWWForm();
        form.AddField("id", PlayerPrefs.GetString("id"));
        form.AddField("levelid", levelid);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        StartCoroutine(GetItems(www, results => OnReceivedModels(results)));
    }

    void OnReceivedModels(ProgressItemModel[] models)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var model in models)
        {
            var instance = GameObject.Instantiate(preFab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            InitializeItemView(instance, model);
        }

    }
    void InitializeItemView(GameObject viewGameObject, ProgressItemModel model)
    {
        ProgressItemView view = new ProgressItemView(viewGameObject.transform);
        view.name.text = model.name;
        view.score.text = "Score: "+ model.score + "/15";
        view.button.onClick.AddListener(
            () =>
            {
                Result.category = model.name;
                Result.resultT = model.score;
                Result.thoeryid = model.id;
                Result.levelid = levelid;

                SceneManager.LoadScene("Level-Def");
            });
    }

    IEnumerator GetItems(UnityWebRequest www, System.Action<ProgressItemModel[]> callback)
    {
        yield return www.SendWebRequest();


        if (www.isNetworkError || www.isHttpError)
        {
            ProgressItemModel[] results = new ProgressItemModel[0];
            //Debug.Log(www.error);
            callback(results);
        }
        else
        {
            if (www.downloadHandler.text.Contains("div"))
            {
                var results = new ProgressItemModel[1];

                for (int i = 0; i < 1; i++)
                {
                    results[i] = new ProgressItemModel
                    {
                        name = "No results",
                        score = 0
                    };
                }
                callback(results);
            }
            else
            {
                this.models = JsonHelper.getJsonArray<ProgressItemModel>(www.downloadHandler.text);
                callback(this.models);
            }
        }
    }

    public class ProgressItemView
    {
        public Text name;
        public Text score;
        public Button button;

        public ProgressItemView(Transform rootView)
        {
            name = rootView.Find("Theory").GetComponent<Text>();
            score = rootView.Find("Score").GetComponent<Text>();
            button = rootView.Find("Button").GetComponent<Button>();
        }

    }

    [System.Serializable]
    public class ProgressItemModel
    {
        public string id;
        public int score;
        public string name;
    }

    public void back()
    {
        SceneManager.LoadScene("Progress");
    }
}
