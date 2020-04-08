using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{

    public Text percent;
    public Text result;
    public Text level;
    public Text categoryT;

    public static int resultT;
    public static string category;
    // Start is called before the first frame update
    void Start()
    {
        
        percent.text = (int)(double.Parse(resultT+"") / double.Parse(15+"") * 100) + "%";
        categoryT.text = category;
        result.text = "Done " + resultT;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void finish() {
        SceneManager.LoadScene("Menu");
    }
}
