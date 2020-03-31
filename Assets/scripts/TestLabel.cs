using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class TestLabel : MonoBehaviour
{

    public Text tname;
    public static string id;
    public new static string name;

    // Start is called before the first frame update
    void Start()
    {
        if (id != null)
        {
            tname.text = name;
        }

    }

    public void l1()
    {
        Testing.id = id;
        Testing.label = "1";
        UnityEngine.SceneManagement.SceneManager.LoadScene(10);
    }
    public void l2()
    {
        Testing.id = id;
        Testing.label = "2";
        UnityEngine.SceneManagement.SceneManager.LoadScene(10);
    }
    public void l3()
    {
        Testing.id = id;
        Testing.label = "3";
        UnityEngine.SceneManagement.SceneManager.LoadScene(10);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
