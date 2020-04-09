using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SimpleJSON;

public class Task : MonoBehaviour
{
    public static string theoryid;

    public GameObject preFab;
    public RectTransform content;

    public JSONArray jsonArray;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetTasks(theoryid));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playBack()
    {
        SceneManager.LoadScene("NewTheory");
    }
    public IEnumerator GetTasks(string theoryid)
    {

        WWWForm form = new WWWForm();
        form.AddField("theoryid", theoryid);

        using (UnityWebRequest www = UnityWebRequest.Post("http://augmented.zzz.com.ua/ar/getTask.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(" ********** " + www.downloadHandler.text);
                this.jsonArray = JSON.Parse(www.downloadHandler.text) as JSONArray;
                new WaitForSeconds(0.1f);

                UpdateItems();
            }
        }
    }

    public void UpdateItems()
    {
        StartCoroutine(GetItems(this.jsonArray.Count, results => OnReceivedModels(results)));
    }

    void OnReceivedModels(TheoryTask[] models)
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

    void InitializeItemView(GameObject viewGameObject, TheoryTask model)
    {

        TaskItemView view = new TaskItemView(viewGameObject.transform);
        view.textGiven.text = model.Task;

        view.btnMore.GetComponentInChildren<Text>().text = "Задание - " + model.Task_Number;
        view.btnLess.GetComponentInChildren<Text>().text = "Задание - " + model.Task_Number;
        
        view.solution.text = model.Solution;
        view.answer.text = "Ответ: " + model.Answer;

        StartCoroutine(LoadImage(view.img, model.Picture));
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

    IEnumerator GetItems(int count, System.Action<TheoryTask[]> callback)
    {
        yield return new WaitForSeconds(0.0001f);
        var results = new TheoryTask[count];

        for (int i = 0; i < jsonArray.Count; i++)
        {
            results[i] = new TheoryTask
            {
                Task = jsonArray[i].AsObject["Task"],
                Solution = jsonArray[i].AsObject["Solution"],
                Answer = jsonArray[i].AsObject["Answer"],
                Picture = jsonArray[i].AsObject["Picture"],
                Task_Number = jsonArray[i].AsObject["Task_Number"]
            };
        }
        callback(results);
    }

    public class TaskItemView
    {
        public Text textGiven;
        public Button btnMore;
        public Button btnLess;
        public RawImage img;
        public Text solution;
        public Text answer;

        public TaskItemView(Transform rootView)
        {
            Transform Given = rootView.GetChild(2);
            Transform Solve = rootView.GetChild(3);

            btnMore = rootView.Find("btnMore").GetComponent<Button>();
            btnLess = rootView.Find("btnLess").GetComponent<Button>();

            textGiven = Given.Find("Text").GetComponent<Text>();

            img = Solve.Find("img").GetComponent<RawImage>();
            solution = Solve.Find("Text").GetComponent<Text>();
            answer = Solve.Find("Answer").GetComponent<Text>();


        }

    }

    [System.Serializable]
    public class TheoryTask
    {
        public string Task_Number;
        public string Task;
        public string Solution;
        public string Picture;
        public string Answer;
    }
}
