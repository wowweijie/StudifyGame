using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreGetter : MonoBehaviour
{
    public List<string> userIds;
    public List<float> totalScores;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetScore());
    }

    IEnumerator GetScore()
    {
        string resultsURL = "http://localhost:5000/CampaignResults/";
        //string resultsURL = "http://localhost:5000/questions/6033a7768d9e9fffb916c03a";
        Debug.Log("Retrieving results");
        UnityWebRequest request = UnityWebRequest.Get(resultsURL);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        // Checks the correctness of Result class (a single object).
        // If the attributes are wrong / incomplete, the following lines wouldn't parse.
        //string singleJson = request.downloadHandler.text.Substring(1, 440);
        //Result result = JsonUtility.FromJson<Result>(singleJson);
        //Debug.Log(result.smScore);

        // https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
        // If this is a Json array from the server and you did not create it by hand:
        string value = "{\"Items\":" + request.downloadHandler.text + "}";
        Result[] results = JsonHelper.FromJson<Result>(value);
        //Debug.Log(results.Length);

        foreach (Result result in results)
        {
            result.computeTotalScore();
        }

        Array.Sort(results, (left, right) => right.totalScore.CompareTo(left.totalScore));

        // Now the results is sorted from highest to lowest totalscore
        //foreach (Result result in results)
        //{
        //    Debug.Log(result.userId);
        //    Debug.Log(result.totalScore);
        //}

        userIds = new List<string>();
        totalScores = new List<float>();
        foreach (Result result in results)
        {
            userIds.Add(result.userId);
            totalScores.Add(result.totalScore);
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
