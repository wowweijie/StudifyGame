using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Video;
using UnityEngine;
using SimpleJSON;

public class VideoManager : MonoBehaviour
{
    public YoutubePlayer.YoutubePlayer youtubePlayer;

    public string youtubeDlURL;


    // Start is called before the first frame update
    void Start()
    {
        YoutubePlayer.YoutubeDl.ServerUrl = youtubeDlURL;
        StartCoroutine(GetCampaignVideo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetCampaignVideo(){

        string categoryURI = UnityWebRequest.EscapeURL(PlayerLevel.category);
        
        string videoQuery = "http://localhost:5000/resources/campaignvideo?category=" + categoryURI;

        Debug.Log("sending api: " + videoQuery);
        UnityWebRequest questionRequest = new UnityWebRequest(videoQuery, "GET");
        questionRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        questionRequest.SetRequestHeader("Content-Type", "application/json");

        yield return questionRequest.SendWebRequest();

        if(questionRequest.responseCode==200){
            var responseBody = JSON.Parse(questionRequest.downloadHandler.text);
            Debug.Log($"youtube link : {responseBody}");
            youtubePlayer.youtubeUrl = responseBody["url"];
            youtubePlayer.PlayVideoAsync();
        }

        else if (questionRequest.responseCode==401){
            Debug.Log("Fail to retrieve question");
        }

        else if (questionRequest.responseCode==500){
            Debug.Log(questionRequest.downloadHandler.text);
        }

        if (questionRequest.isNetworkError || questionRequest.isHttpError) {
            Debug.LogError(questionRequest.error);
            yield break;
        }

        
    }
}
