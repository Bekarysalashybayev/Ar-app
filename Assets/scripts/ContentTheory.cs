using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SimpleJSON;

public class ContentTheory : MonoBehaviour
{
    public GameObject preFab;
    public RectTransform content;
    public string id;
    public JSONArray jsonArray;

    public  GameObject testGameObject;
    public  GameObject theoryGameObject;


    public new InputField name;

    public string lastVal = "";
    public bool start = true;

    TeoriesItemModel[] models;

    private void Start()
    {
        UpdateItems();
    }

    private void Update()
    {
        if (this.name.text != "")
        {
            if ( this.name.text != this.lastVal)
            {
                string url = "http://augmented.zzz.com.ua/ar/getTheoriesLike.php";
                WWWForm form = new WWWForm();
                form.AddField("name", this.name.text);
                UnityWebRequest www = UnityWebRequest.Post(url, form);
                StartCoroutine(GetItems(www, results => OnReceivedModels(results)));

                this.lastVal = this.name.text;
                this.start = true;
            }
            
        }
        else
        {
            if (start == true)
            {
                UpdateItems();
            }
            this.start = false;
        }
    }

    public void UpdateItems()
    {
        string url = "http://augmented.zzz.com.ua/ar/getTheories.php";
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        StartCoroutine(GetItems(www, results => OnReceivedModels(results)));
    }

    void OnReceivedModels(TeoriesItemModel[] models)
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

    void InitializeItemView(GameObject viewGameObject, TeoriesItemModel model)
    {
        if(model.id =="0")
        {
            viewGameObject.SetActive(false);
        }
        else
        {
            TeoriesItemView view = new TeoriesItemView(viewGameObject.transform);
            view.textTitle.text = model.name;
            view.tButton.onClick.AddListener(
                () =>
                {
                    Scene scene = SceneManager.GetActiveScene();
                    if (scene.name == "NewTheory")
                    {
                        if (theoryGameObject.activeSelf)
                        {
                            SceneManager.LoadScene("NewThoryDesc");
                           // Debug.Log(model.id + "is clicked by theory");
                        }
                        else if (testGameObject.activeSelf)
                        {
                            Debug.Log(model.id + "is clicked by test");
                        }
                    }
                    else
                    {
                        Level.category = model.name;
                        Level.theoryid = model.id;
                        Result.category = model.name;
                        SceneManager.LoadScene("Level");
                    }
                    

                }
                );
        }
       

    }

    IEnumerator GetItems(UnityWebRequest www, System.Action<TeoriesItemModel[]> callback)
    {
        yield return www.SendWebRequest();
        

        if (www.isNetworkError || www.isHttpError)
        {
            TeoriesItemModel[] results = new TeoriesItemModel[0];
            //Debug.Log(www.error);
            callback(results);
        }
        else
        {
            if (www.downloadHandler.text == "0")
            {
                var results = new TeoriesItemModel[1];

                for (int i = 0; i < 1; i++)
                {
                    results[i] = new TeoriesItemModel
                    {
                        id = "0",
                        name = "No results"
                    };
                }
                callback(results);
            }
            else
            {
                this.models = JsonHelper.getJsonArray<TeoriesItemModel>(www.downloadHandler.text);
               // Debug.Log(www.downloadHandler.text);
                callback(this.models);
            }     
        }
    }

    public class TeoriesItemView
    {
        public Text textTitle;
        public Button tButton;

        public TeoriesItemView(Transform rootView)
        {
            textTitle = rootView.Find("NameTheory").GetComponent<Text>();
            tButton = rootView.Find("TButton").GetComponent<Button>();
        }

    }

    [System.Serializable]
    public class TeoriesItemModel
    {
        public string name;
        public string id;
    }

    public class JsonHelper
    {
        public static T[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        public static string arrayToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.array = array;
            return JsonUtility.ToJson(wrapper);
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }   
}