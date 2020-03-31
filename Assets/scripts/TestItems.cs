using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestItems : MonoBehaviour
{
    public InputField InputAnswer;
    public Button Check;
    public Text Error;
    public Text Answer;
    public RawImage RightAnswer;
    public GameObject itemBought;

    void Start()
    {
        Check.onClick.AddListener(
           () =>
           {
               if ("Ответ: " + InputAnswer.text == Answer.text)
               {
                   Error.text = "";
                   itemBought.SetActive(true);
               }
               else
               {
                   itemBought.SetActive(false);
                   Error.text = "Error";
                   Error.color = Color.red;
               }
           }
           );
    }
}