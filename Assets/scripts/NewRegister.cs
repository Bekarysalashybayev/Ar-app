using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System;

public class NewRegister : MonoBehaviour
{

    public bool emailExist;

    public GameObject reg1;
    public GameObject reg2;
    public GameObject reg3;

    public GameObject btngo;
    public GameObject codego;

    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;

    public InputField first;
    public InputField last;
    public InputField email;
    public InputField code;
    public InputField password;
    public InputField cPassword;

    public string codeToSend = "556644";

    private void Start()
    {
        btn1.onClick.AddListener(
                () =>
                {
                    Transform firstgc = reg1.transform.GetChild(2);
                    Text error = firstgc.Find("Error").GetComponent<Text>();
                    Image image = firstgc.Find("Image").GetComponent<Image>();

                    Transform lastgc = reg1.transform.GetChild(4);
                    Text error2 = lastgc.Find("Error2").GetComponent<Text>();
                    Image image2 = lastgc.Find("Image").GetComponent<Image>();

                    if (first.text == "")
                    {
                        error.text = "FirstName empty";
                        image.color = Color.red;
                    }
                    else if (last.text == "")
                    {
                        error2.text = "LastName empty";
                        image2.color = Color.red;
                        error.text = "";
                        image.color = Color.blue;
                    }
                    else
                    {
                        error2.text = "";
                        image2.color = Color.blue;
                        reg1.SetActive(false);
                        reg2.SetActive(true);
                    }


                }
                );
        btn2.onClick.AddListener(
                () =>
                {
                    Transform emailgc = reg2.transform.GetChild(2);
                    Text error3 = emailgc.Find("Error3").GetComponent<Text>();
                    Image image3 = emailgc.Find("Image").GetComponent<Image>();

                    if (email.text == "")
                    {
                        error3.text = "Email empty";
                        image3.color = Color.red;
                    }
                    else
                    {
                        string url = "https://bekarysalashybaev.000webhostapp.com/arrapp/checkEmail.php";
                        WWWForm form = new WWWForm();
                        form.AddField("loginUser", email.text);
                        UnityWebRequest www = UnityWebRequest.Post(url, form);
                        StartCoroutine(checkEmailExists(www, error3, image3));
                    }
                   
                }
                );
        btn3.onClick.AddListener(
                () =>
                {
                    Transform codgc = reg2.transform.GetChild(4);
                    Text error4 = codgc.Find("Error4").GetComponent<Text>();
                    Image image4 = codgc.Find("Image").GetComponent<Image>();

                    if (code.text != codeToSend)
                    {
                        error4.text = "Code error";
                        image4.color = Color.red;
                    }
                    else
                    {
                        error4.text = "";
                        image4.color = Color.blue;
                        new WaitForSeconds(0.1f);
                        reg2.SetActive(false);
                        reg3.SetActive(true);
                    }
                    

                }
                );
        btn4.onClick.AddListener(
                () =>
                {
                    Transform passtgc = reg3.transform.GetChild(2);
                    Text error = passtgc.Find("Error5").GetComponent<Text>();
                    Image image = passtgc.Find("Image").GetComponent<Image>();

                    Transform congc = reg3.transform.GetChild(4);
                    Text error2 = congc.Find("Error6").GetComponent<Text>();
                    Image image2 = congc.Find("Image").GetComponent<Image>();

                    if (password.text == "")
                    {
                        error.text = "Password empty";
                        image.color = Color.red;
                    }
                    else if (cPassword.text != password.text)
                    {
                        error2.text = "Password is doesnt match";
                        image2.color = Color.red;
                        error.text = "";
                        image.color = Color.blue;
                    }
                    else
                    {
                        error2.text = "";
                        image2.color = Color.blue;
                        new WaitForSeconds(1f);

                        StartCoroutine(Register(first.text, last.text, email.text, password.text));
                        
                    }
                }
                );
    }

    IEnumerator Register(string firstName, string lastName, string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("firstname", firstName);
        form.AddField("lastname", lastName);
        form.AddField("email", email);
        form.AddField("password", password);

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
                    PlayerPrefs.SetString("userName", email);
                    PlayerPrefs.SetString("password", password);

                    SceneManager.LoadScene(4);
                }
                else
                {
                    
                }
            }
        }
    }

    IEnumerator checkEmailExists(UnityWebRequest www, Text error3, Image image3)
    {
        yield return www.SendWebRequest();


        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            this.emailExist = false;
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.downloadHandler.text == "username exists")
            {

                error3.text = "Email already used";
            }
            else
            {
                error3.text = "";
                image3.color = Color.blue;
                StartCoroutine(sendCodeToMail(email.text));
                btngo.SetActive(true);
                codego.SetActive(true);
            }
        }
    }

    IEnumerator sendCodeToMail(string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("code", this.codeToSend);

        using (UnityWebRequest www = UnityWebRequest.Post("https://bekarysalashybaev.000webhostapp.com/arrapp/confirmEmail.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
              Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
