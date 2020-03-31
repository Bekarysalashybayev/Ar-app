using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class Items : MonoBehaviour
{
    Action<string> _createItemsCallback;
    // Start is called before the first frame update
    void Start()
    {
        _createItemsCallback = (jsonArrayString) => {
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };

        CreateItems();
    }

    public void CreateItems() {
        StartCoroutine(getItemsIDS("1", _createItemsCallback));
    }

    IEnumerator CreateItemsRoutine(string jsonArrayString) {

        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for ( int i=0; i< jsonArray.Count; i++) {
            bool isDone = false;
            string itemId = jsonArray[i].AsObject["itemID"];
            JSONObject itemInfoJson = new JSONObject();

            Action<string> getItemInfoCallback = (itemInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
            };

            StartCoroutine(getItems(itemId, getItemInfoCallback));
            yield return new WaitUntil(() => isDone == true);

            GameObject item = Instantiate(Resources.Load("Prefabs/item") as GameObject);
            item.transform.SetParent(this.transform);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            item.transform.Find("name").GetComponent<Text>().text = itemInfoJson["name"];
            item.transform.Find("price").GetComponent<Text>().text = itemInfoJson["price"];
            item.transform.Find("description").GetComponent<Text>().text = itemInfoJson["description"];

        }
    }


    public IEnumerator getItemsIDS(string userID, System.Action<string> callback)
    {

        WWWForm form = new WWWForm();
        form.AddField("userID", userID);

        using (UnityWebRequest www = UnityWebRequest.Post("https://bekarysalashybaev.000webhostapp.com/arrapp/printDates.php", form))
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



    public IEnumerator getItems(string itemID, System.Action<string> callback)
    {

        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);

        using (UnityWebRequest www = UnityWebRequest.Post("https://bekarysalashybaev.000webhostapp.com/arrapp/getItems.php", form))
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
