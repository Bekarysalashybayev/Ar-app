using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class getitems : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

   public IEnumerator getItems(System.Action<string> callback)
    {

        using (UnityWebRequest www = UnityWebRequest.Get("https://bekarysalashybaev.000webhostapp.com/arrapp/printDates.php"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;
                callback(jsonArray);
            }
        }
    }
}
