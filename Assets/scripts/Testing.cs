using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using System;

public class Testing : MonoBehaviour
{
    public static string id;
    public static string label;

    public GameObject preFab;
    public RectTransform content;
    public JSONArray jsonArray;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetTasks(id,label));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GetTasks(string id, string label)
    {

        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("label", label);

        using (UnityWebRequest www = UnityWebRequest.Post("https://arapp-1601.000webhostapp.com/arr/api/task.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(" ********** " +www.downloadHandler.text);
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

        TeoriesItemView view = new TeoriesItemView(viewGameObject.transform);
        view.textGiven.text = model.given;
        view.textButtonM.text = model.mButtonText;
        view.textButtonL.text = model.lbuttonText;
        view.textLabelM.text = model.mLabelText;
        view.textLabell.text = model.lLabelText;
        view.textSolution.text = model.solution;
        view.textAnswer.text = model.answer;
        StartCoroutine(LoadImage(view.imgSolution, model.solutionImg));

       /* view.Check.onClick.AddListener(
           () =>
           {
               if (view.InputAnswer.text.ToLower().Equals(model.answer.ToLower()))
               {
                   view.Error.text = "Rigth";
                   view.Error.color = Color.green;
                   view.RightAnswer.enabled = true;
               }
               else
               {
                   view.RightAnswer.enabled = false;
                   view.Error.text = "Error";
                   view.Error.color = Color.red;
               }
               Debug.Log(view.InputAnswer.text);
               Debug.Log(model.answer);

               Console.Write(model.answer);
           }
           );*/


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

    IEnumerator GetItems(int count, System.Action<TeoriesItemModel[]> callback)
    {
        yield return new WaitForSeconds(0.0001f);
        var results = new TeoriesItemModel[count];

        for (int i = 0; i < jsonArray.Count; i++)
        {
            results[i] = new TeoriesItemModel
            {
              given = jsonArray[i].AsObject["given"],
               mButtonText = "Задание " + (i + 1),
                lbuttonText = "Задание " + (i + 1),
                mLabelText = getLabel(jsonArray[i].AsObject["label"].ToString()),
                lLabelText = getLabel(jsonArray[i].AsObject["label"].ToString()),
                solutionImg = jsonArray[i].AsObject["img"],
                solution = jsonArray[i].AsObject["solution"],
                answer = "Ответ: " + jsonArray[i].AsObject["answer"] 
            };
        }
        callback(results);
    }

    private string getLabel(string label)
    {
        string labels = "";
        if (label.Contains("1"))
        {
            return "Уровень: Легкий";
        }
        else if (label.Contains("2"))
        {
            labels = "Уровень: Средний";
        }
        else
        {
            labels = "Уровень: Сложный";
        }
        return labels;
    }

    public class TeoriesItemView
    {
       public Text textGiven;
       public Text textButtonM;
        public Text textButtonL;
        public Text textLabelM;
        public Text textLabell;
        public Text textSolution;
        public Text textAnswer;
        public RawImage imgSolution;

        public InputField InputAnswer;
        public Button Check;
        public Text Error;
        public RawImage RightAnswer;

        public TeoriesItemView(Transform rootView)
        {
            Transform solve = rootView.GetChild(2);
            Transform more = rootView.GetChild(0);
            Transform less = rootView.GetChild(1);

            Transform solution = solve.GetChild(3);

            Transform Panel1 = more.GetChild(2);

            Transform Panel2 = less.GetChild(2);

           // Debug.Log("Child found. Mame: " + Panel1.name);
          //  Debug.Log("Child found. Mame: " + Panel2.name);
            textGiven = solve.Find("Given").GetComponent<Text>();

            textButtonM = more.Find("textButtonM").GetComponent<Text>();
            textLabelM = Panel1.Find("textLabelM").GetComponent<Text>();

            textButtonL = less.Find("textButtonL").GetComponent<Text>();
            textLabell = Panel2.Find("textLabell").GetComponent<Text>();

              textSolution = solution.Find("SolutionText").GetComponent<Text>();
              textAnswer = solution.Find("answer").GetComponent<Text>();

              imgSolution = solution.Find("SolutionImg").GetComponent<RawImage>();

            InputAnswer = solve.GetChild(2).Find("InputAnswer").GetComponent<InputField>();
            Check = solve.GetChild(2).Find("Check").GetComponent<Button>();
            Error = solve.GetChild(2).Find("Error").GetComponent<Text>();
            RightAnswer = solve.GetChild(2).Find("RightAnswer").GetComponent<RawImage>();
        }

    }
    public class TeoriesItemModel
    {
      public string given;
      public string mButtonText;
      public string lbuttonText;
      public string mLabelText;
      public string lLabelText;
      public string solutionImg;
      public string solution;
      public string answer;
    }
}
