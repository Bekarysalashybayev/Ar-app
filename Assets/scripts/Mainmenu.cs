using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void Home()
    {
        SceneManager.LoadScene(0);
    }
    public void Explore()
    {
        SceneManager.LoadScene(5);
    }

    public void Scan()
    {
        SceneManager.LoadScene(1);
    }

    public void Testing()
    {
        SceneManager.LoadScene(8);
    }

    public void Profile()
    {
        SceneManager.LoadScene(2);
    }

    public void Theory()
    {
        SceneManager.LoadScene(4);
    }
}
