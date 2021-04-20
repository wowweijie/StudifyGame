using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON; 



public class LoginManager : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;

    public GameObject AuthFailPrefab;

    public Transform canvas;

    public GameObject loginFailPopup;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLogin()
    {
        if (string.IsNullOrEmpty(usernameField.text) | string.IsNullOrEmpty(passwordField.text)){
            GameObject popupMsg = Instantiate(loginFailPopup);
            popupMsg.GetComponent<PopupManager>().Init(GameObject.Find("Canvas").transform);
        } else {
            StartCoroutine(Authenticate(usernameField.text, passwordField.text));
        }
        
    }

    IEnumerator Authenticate(string username, string password){
        string loginURL = "http://localhost:5000/students/login/?username=" + username + "&password=" + password;

        Debug.Log("sending api: " + loginURL);
        UnityWebRequest loginRequest = UnityWebRequest.Get(loginURL);

        yield return loginRequest.SendWebRequest();

        if(loginRequest.responseCode==201){
            var userInfo = JSON.Parse(loginRequest.downloadHandler.text);
            PlayerLevel.userId = userInfo["_id"].Value;
            PlayerLevel.userName = userInfo["username"].Value;
            SceneManager.LoadScene("IntroMenu");
        }

        else if (loginRequest.responseCode==401){
            GameObject popUp = Instantiate(AuthFailPrefab);
            Debug.Log(popUp);
            popUp.GetComponent<PopupManager>().Init(canvas);
        }

        if (loginRequest.isNetworkError || loginRequest.isHttpError) {
            Debug.LogError(loginRequest.error);
            yield break;
        }

        
    }
}
