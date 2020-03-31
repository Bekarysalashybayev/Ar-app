using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class register : MonoBehaviour
{
    public InputField UserNameInput;
    public InputField PasswordInput;
    public Button RegisterButton;
    public Text ErrorText;

    // Start is called before the first frame update
    void Start()
    {
        RegisterButton.onClick.AddListener(() => {
            StartCoroutine(Login(UserNameInput.text, PasswordInput.text));
        });
    }

    IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("https://bekarysalashybaev.000webhostapp.com/arrapp/register.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text.Equals("New record created successfully"))
                {
                    SceneManager.LoadScene(0);
                }
                else
                {
                    ErrorText.text = www.downloadHandler.text;
                }
            }
        }
    }
}
