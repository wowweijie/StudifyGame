using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;
  
[System.Serializable]
public struct dropdownMap
{
    public Dropdown category;
    public Dropdown difficulty;
}

public class CreateManager : MonoBehaviour
{
    public static string createdGameId;
    public List<dropdownMap> questionConfigs;

    public GameObject popupPrefab;

    public Transform canvas;

    [System.Serializable]
    public class Question {
        public string category;
        public string difficulty;
    }

    [System.Serializable]
    public class Questions{
        public Question[] data;
    }
    
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCreate()
    {
        Questions questions = new Questions();
        questions.data = new Question[5];
        int i = 0;
        foreach (dropdownMap row in questionConfigs){
            Debug.Log(row.category.options[row.category.value].text);
            Debug.Log(row.difficulty.options[row.difficulty.value].text);
            questions.data[i] = new Question();
            questions.data[i].category = row.category.options[row.category.value].text;
            questions.data[i].difficulty = row.difficulty.options[row.difficulty.value].text;
            i++;
        };
        Debug.Log(JsonUtility.ToJson(questions, true));
        string jsonBody = JsonUtility.ToJson(questions);
        StartCoroutine(CreateAssignment(jsonBody));

        
    }

    IEnumerator CreateAssignment(string jsonBody){
        string createURL = "http://localhost:5000/assignments/player_create";

        Debug.Log("sending api: " + createURL);
        var request = new UnityWebRequest(createURL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if(request.responseCode==201){
            // GameObject popUp = Instantiate(popupPrefab);
            // Debug.Log(popUp);
            // popUp.GetComponent<PopupManager>().Init(canvas);
            var responseBody = JSON.Parse(request.downloadHandler.text);
            Debug.Log(responseBody);
            createdGameId = responseBody["_id"];
            SceneManager.LoadScene("SocialMediaScene");
        }

        else if (request.responseCode==401){
            
        }

        if (request.isNetworkError || request.isHttpError) {
            Debug.LogError(request.error);
            yield break;
        }

        
    }
}
