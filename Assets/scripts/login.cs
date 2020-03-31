using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class login : MonoBehaviour
{
    public InputField UserNameInput;
    public InputField PasswordInput;
    public Button LoginButton;
    public Text ErrorText;
    

    // Start is called before the first frame update
    void Start()
    {
        LoginButton.onClick.AddListener(() => {
            StartCoroutine(Login(UserNameInput.text, PasswordInput.text));
        });
    }

    IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("https://bekarysalashybaev.000webhostapp.com/arrapp/getDate.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text.Equals("login Success"))
                {

                    SceneManager.LoadScene(0);
                }
                else {
                    ErrorText.text = www.downloadHandler.text   ;
                }
            }
        }
    }

  public void setUserName(string username){

  }

}
