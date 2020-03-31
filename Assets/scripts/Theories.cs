using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections.Generic;


public class Theories : MonoBehaviour
{
    public GameObject preFab;
    public RectTransform content;
    public string id;
    public JSONArray jsonArray;
    private void Start()
    {
        StartCoroutine(GetNames());
    }

    public IEnumerator GetNames()
    {

        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Post("https://arapp-1601.000webhostapp.com/arr/api/theoryname.php", form))
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
                UpdateItems();
            }
        }
    }
    public void UpdateItems()
    {
        StartCoroutine(GetItems(this.jsonArray.Count, results => OnReceivedModels(results)));
    }

     void OnReceivedModels(TeoriesItemModel[] models)
    {
        foreach ( Transform child in content)
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
        TeoriesItemView view = new TeoriesItemView(viewGameObject.transform);
        view.textTitle.text = model.title;
        view.tButton.GetComponentInChildren<Text>().text = model.buttonText;
        view.tButton.onClick.AddListener(
            ()=>
            {
                Scene scene = SceneManager.GetActiveScene();
                if (scene.name == "Theory")
                {
                    TheoryDetail.id = model.id;
                    Debug.Log("clicked" + model.id);
                    SceneManager.LoadScene(7);
                }
                else if (scene.name == "Test")
                {
                    TestLabel.id = model.id;
                    TestLabel.name = model.title;
                    SceneManager.LoadScene(9);
                }
                
            }
            );

    }

    IEnumerator GetItems(int count, System.Action<TeoriesItemModel[]> callback)
    {
        yield return new WaitForSeconds(0.0001f);
        var results = new TeoriesItemModel[count];

        for (int i = 0; i < jsonArray.Count; i++)
        {
            results[i] = new TeoriesItemModel
            {
                title = jsonArray[i].AsObject["name"],
                buttonText = "",
                id = jsonArray[i].AsObject["id"]
            };
        }
        callback(results);
    }

    public class TeoriesItemView
    {
        public Text textTitle;
        public Button tButton;

        public TeoriesItemView(Transform rootView)
        {
            textTitle = rootView.Find("TitleText").GetComponent<Text>();
            tButton = rootView.Find("TButton").GetComponent<Button>();
        }

    }

    public class TeoriesItemModel {
        public string title;
        public string buttonText;
        public string id;
    }
}
