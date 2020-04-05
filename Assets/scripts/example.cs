using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class example : MonoBehaviour
{

    public RawImage img;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadImage(img, "https://arapp-1601.000webhostapp.com/arr/api/photo5197358183251815875.jpg"));
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
