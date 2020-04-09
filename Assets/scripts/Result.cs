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
    public Text LevelT;

    public static int resultT;
    public static string category;
    public static string levelid;
    public static string thoeryid;
    // Start is called before the first frame update
    void Start()
    {
        
        percent.text = (int)(double.Parse(resultT+"") / double.Parse(15+"") * 100) + "%";
        categoryT.text = category;
        switch (levelid)
        {
            case "1":
                LevelT.text = "Easy";
                break;
            case "2":
                LevelT.text = "Medium";
                break;
            case "3":
                LevelT.text = "Hard";
                break;
        }
        result.text = "Done " + resultT;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void finish() {
        SceneManager.LoadScene("Menu");
    }
    public void startAgain()
    {
        Testing.category = category;
        Testing.level = levelid;
        Testing.theoryid = thoeryid;
        SceneManager.LoadScene("Testing");
    }
}
