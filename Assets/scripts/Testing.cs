using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SimpleJSON;
using static ContentTheory;
using System;
using System.Collections.Generic;

public class Testing : MonoBehaviour
{
    public static string category;
    public static string level;
    public static string theoryid;

    public GameObject content;

    public GameObject navTest;
    public RectTransform navContent;

    public GameObject bodyTest;
    public RectTransform bodyContent;

    public GameObject prev;
    public GameObject next;
    public Button btn;
    public Text message;

    public TestNumber[] testNumbers = new TestNumber[15];
    public Answered[] answereds = new Answered[15];

    Vector3 pos;

    private int localTestNumber = 0;

    private int scoreFirst = 0;

    private bool changetest = true;

    public bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        //this.pos = navContent.transform.position;
        Debug.Log("***  " + level + " " +theoryid);

        for (int i = 0; i < 15; i++)
         {
            this.answereds[i].answer = "";
        }

        if (first == true)
        {
            startTest();
            StartCoroutine(GetScore());
        }
       

        
    }

    public void back()
    {
        Level.category = category;
        Level.theoryid = theoryid;
        SceneManager.LoadScene("Level");
    }
    void Update()
    {
       if (first == false)
        {
            if (this.changetest == true)
            {
                UpdateItems(testNumbers);
                this.changetest = false;
                if (this.localTestNumber > 3)
                {
                    navContent.transform.position = new Vector3(220.00f - (this.localTestNumber) / 2, 651.8f, 98.3f);


                }
                else
                {
                    navContent.transform.position = new Vector3(220.00f, 651.8f, 98.3f);
                }


            }

            if (this.localTestNumber == 0)
            {
                prev.SetActive(false);
            }
            else
            {
                prev.SetActive(true);
                next.SetActive(true);
            }
        }
        else {
            startTest();
        }
        
        



    }
    public void nextN()
    {

        

        if (this.localTestNumber < (this.testNumbers.Length-2))
        {
            message.text = "";
            btn.GetComponentInChildren<Text>().text = "Next";
            this.localTestNumber++;
            this.changetest = true;
            OnReceivedBodyTest(testNumbers[this.localTestNumber]);
        }
        else if (this.localTestNumber == (this.testNumbers.Length - 2))
        {
            message.text = "Если твой балл будет больше прежнего то ответ обновляется";
            btn.GetComponentInChildren<Text>().text = "Finish";
            this.localTestNumber++;
            this.changetest = true;
            OnReceivedBodyTest(testNumbers[this.localTestNumber]);
        }
        else
        {
            save();
        }
        

    }

    public void prevN()
    {
        this.localTestNumber--;
        this.changetest = true;
        OnReceivedBodyTest(testNumbers[this.localTestNumber]);
    }

    public void save()
    {
        int score = 0;
        for (int i=0; i < this.answereds.Length; i++)
        {
            if (this.answereds[i].answer == this.testNumbers[i].Answer)
            {
                score++;
                //Debug.Log(this.answereds[i].answer);
                //Debug.Log(this.testNumbers[i].Answer);
            }
        }

        if (score > this.scoreFirst)
        {
            string url = "http://augmented.zzz.com.ua/ar/saveResult.php";
            WWWForm form = new WWWForm();
            form.AddField("id", PlayerPrefs.GetString("id"));
            form.AddField("theoryid", theoryid);
            form.AddField("levelid", level);
            form.AddField("score", score);
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            StartCoroutine(SaveScore(www)); Debug.Log(score);
        }
        Result.resultT = score;

        Result.thoeryid = theoryid;
        Result.levelid = level;
        SceneManager.LoadScene("Level-Def");
       
    }

    public void startTest()
    {
        string url = "http://augmented.zzz.com.ua/ar/getTest.php";
        WWWForm form = new WWWForm();
        form.AddField("levelid", level);
        form.AddField("theoryid", theoryid);
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        StartCoroutine(GetItems(www));
    }

    public void UpdateItems(TestNumber[] testNumbers)
    {
        OnReceivedNavTest(testNumbers);
        OnReceivedBodyTest(testNumbers[this.localTestNumber]);
    }

    void OnReceivedNavTest(TestNumber[] models)
    {
        foreach (Transform child in navContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var model in models)
        {
            var instance = GameObject.Instantiate(navTest.gameObject) as GameObject;
            instance.transform.SetParent(navContent, false);
            InitializeItemView(instance, model);
        }

    }
    void OnReceivedBodyTest(TestNumber models)
    {
        foreach (Transform child in bodyContent)
        {
            Destroy(child.gameObject);
        }

        var instance = GameObject.Instantiate(bodyTest.gameObject) as GameObject;
            instance.transform.SetParent(bodyContent, false);
            InitializeItemContent(instance, models);

    }
    void InitializeItemView(GameObject viewGameObject, TestNumber model)
    {
            TeoriesItemView view = new TeoriesItemView(viewGameObject.transform);
            view.tButton.GetComponentInChildren<Text>().text = model.number;
        

        if (this.answereds[model.localTestNumber].answer != "")
        {
            view.tButton.GetComponent<Image>().color = Color.grey;
        }

        if (this.localTestNumber == model.localTestNumber)
        {
            view.tButton.GetComponent<Image>().color = Color.green;
        }

        view.tButton.onClick.AddListener(
                () =>
                {
                    this.localTestNumber = model.localTestNumber;
                    this.changetest = true;
                    OnReceivedBodyTest(model);
                }
                );
     }


    public static List<T> Randomize<T>(List<T> list)
    {
        List<T> randomizedList = new List<T>();
        System.Random rnd = new System.Random();
        while (list.Count > 0)
        {
            int index = rnd.Next(0, list.Count); //pick a random item from the master list
            randomizedList.Add(list[index]); //place it at the end of the randomized list
            list.RemoveAt(index);
        }
        return randomizedList;
    }



    void InitializeItemContent(GameObject viewGameObject, TestNumber model)
    {
        List<String> list = new List<String>();
        list.Add(model.Answer);
        list.Add(model.Choice1);
        list.Add(model.Choice2);
        list.Add(model.Choice3);

        list = Randomize<String>(list);

        TestItemView view = new TestItemView(viewGameObject.transform);
         view.text.text = model.Task;

        view.Answer.GetComponentInChildren<Text>().text = list[0];
        view.Answer1.GetComponentInChildren<Text>().text = list[1];
        view.Answer2.GetComponentInChildren<Text>().text = list[2];
        view.Answer3.GetComponentInChildren<Text>().text = list[3];
         if (view.Answer.GetComponentInChildren<Text>().text == this.answereds[this.localTestNumber].answer)
          {
            view.Answer.GetComponent<Image>().color = Color.green;
          }
         else if (view.Answer1.GetComponentInChildren<Text>().text == this.answereds[this.localTestNumber].answer)
         {
            view.Answer1.GetComponent<Image>().color = Color.green;
         }
         else if (view.Answer2.GetComponentInChildren<Text>().text == this.answereds[this.localTestNumber].answer)
         {
            view.Answer2.GetComponent<Image>().color = Color.green;
         }
        else if (view.Answer3.GetComponentInChildren<Text>().text == this.answereds[this.localTestNumber].answer)
        {
            view.Answer3.GetComponent<Image>().color = Color.green;
        }

        view.Answer.onClick.AddListener(
                () =>
                {
                    view.Answer.GetComponent<Image>().color = Color.green;
                    view.Answer1.GetComponent<Image>().color = Color.white;
                    view.Answer2.GetComponent<Image>().color = Color.white;
                    view.Answer3.GetComponent<Image>().color = Color.white;
                    this.answereds[this.localTestNumber].number = this.localTestNumber;
                    this.answereds[this.localTestNumber].answer = view.Answer.GetComponentInChildren<Text>().text;
                });
        view.Answer1.onClick.AddListener(
                () =>
                {
                    view.Answer1.GetComponent<Image>().color = Color.green;
                    view.Answer.GetComponent<Image>().color = Color.white;
                    view.Answer2.GetComponent<Image>().color = Color.white;
                    view.Answer3.GetComponent<Image>().color = Color.white;

                    this.answereds[this.localTestNumber].number = this.localTestNumber;
                    this.answereds[this.localTestNumber].answer = view.Answer1.GetComponentInChildren<Text>().text;
                });
        view.Answer2.onClick.AddListener(
                () =>
                {
                    view.Answer2.GetComponent<Image>().color = Color.green;
                    view.Answer1.GetComponent<Image>().color = Color.white;
                    view.Answer.GetComponent<Image>().color = Color.white;
                    view.Answer3.GetComponent<Image>().color = Color.white;

                    this.answereds[this.localTestNumber].number = this.localTestNumber;
                    this.answereds[this.localTestNumber].answer = view.Answer2.GetComponentInChildren<Text>().text;
                });
        view.Answer3.onClick.AddListener(
                () =>
                {
                    view.Answer3.GetComponent<Image>().color = Color.green;
                    view.Answer1.GetComponent<Image>().color = Color.white;
                    view.Answer2.GetComponent<Image>().color = Color.white;
                    view.Answer.GetComponent<Image>().color = Color.white;

                    this.answereds[this.localTestNumber].number = this.localTestNumber;
                    this.answereds[this.localTestNumber].answer = view.Answer3.GetComponentInChildren<Text>().text;
                });

    }
    // Update is called once per frame

    IEnumerator GetItems(UnityWebRequest www)
    {
        yield return www.SendWebRequest();


        if (www.isNetworkError || www.isHttpError)
        {
            TestNumber[] results = new TestNumber[0];
            Debug.Log(www.error);
        }
        else
        {
            if (www.downloadHandler.text != "0")
            {
                this.testNumbers = JsonHelper.getJsonArray<TestNumber>(www.downloadHandler.text);
               // Debug.Log(www.downloadHandler.text);
                first = false;
            }
        }
    }
    IEnumerator SaveScore(UnityWebRequest www)
    {
        yield return www.SendWebRequest();


        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.downloadHandler.text != "0")
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetScore()
    {

        WWWForm form = new WWWForm();
        form.AddField("levelid", level);
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
                if (www.downloadHandler.text != "0")
                {
                    JSONArray jsonArray = JSON.Parse(www.downloadHandler.text) as JSONArray;

                    int score = 0;
                    string str = jsonArray[0].AsObject["Score"];
                    String[] strlist = str.Split('"');
                    Int32.TryParse(strlist[0], out score);
                    this.scoreFirst = score;
                }

            }
        }
    }


    public class TeoriesItemView
    {
        public Button tButton;

        public TeoriesItemView(Transform rootView)
        {
            tButton = rootView.GetComponent<Button>();
        }

    }
    public class TestItemView
    {
        public Text text;

        public Button Answer;
        public Button Answer1;
        public Button Answer2;
        public Button Answer3;

        public TestItemView(Transform rootView)
        {
            text = rootView.Find("Text").GetComponent<Text>();
            Answer = rootView.Find("Answer").GetComponent<Button>();
            Answer1 = rootView.Find("Answer1").GetComponent<Button>();
            Answer2 = rootView.Find("Answer2").GetComponent<Button>();
            Answer3 = rootView.Find("Answer3").GetComponent<Button>();
        }

    }

    [System.Serializable]
    public class TestNumber
    {
        public string number;
        public int localTestNumber;
        public string Task;
        public string Choice1;
        public string Choice2;
        public string Choice3;
        public string Answer;

    }

    [System.Serializable]
    public class Answered
    {
        public int number;
        public string answer;
    }

}
