using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BarChart : MonoBehaviour
{
    public Ranking rankingPrefab;
    public Ranking_Space rankingSpacePrefab;

    List<string> playerIds;
    List<String> playerNames;
    List<float> playerScores;

    List<int> playerAvatars;

    //public String thisPlayerName;
    //public float thisPlayerScore;
    //bool ifTop5 = false;
    int thisPlayerRank;

    public Color thisPlayerColor;
    public Color otherPlayerColor;

    List<Ranking> rankings = new List<Ranking>();
    float chartWidth;

    //public ScoreGetter scoreGetter;

    // Start is called before the first frame update
    
    void Start()
    {
        playerIds = new List<String>();
        playerNames = new List<String>();
        playerScores = new List<float>();
        StartCoroutine(GetScore());
        //// normalizedScores are between 0 to 1
        //List<float> normalizedScores = normalizeScores(playerScores);

        //chartWidth = Screen.width + GetComponent<RectTransform>().sizeDelta.x;

        //// Display rankings
        //DisplayRanking(playerNames, normalizedScores);
    }

    public List<String> getPlayerIds()
    {
        return playerIds;
    }
    void DisplayRanking(List<String> otherPlayerNames , List<float> vals, List<int> avatarLists)
    {
        for (int i = 0; i < vals.Count; i++)
        {
            // transform means this panel, which is newRanking's parent
            Ranking newRanking = Instantiate(rankingPrefab, transform);

            // adjust the bar width, we fix bar to left and use offset from right to change width
            float maxWidth = newRanking.barPanel.rect.width;
            RectTransform rt = newRanking.bar.GetComponent<RectTransform>();
            rt.offsetMax = new Vector2(-(1-vals[i])*maxWidth, rt.offsetMax.y);

            newRanking.playerRank.text = (i+1).ToString();
            newRanking.playerName.text = otherPlayerNames[i];
            Debug.Log(newRanking.avatar);
            Image sp = newRanking.avatar.GetComponent<Image>() ;
            Debug.Log(AvatarFactory.Instance.getAvatar(avatarLists[i]));
            Debug.Log(avatarLists[i]);
            sp.sprite = AvatarFactory.Instance.getAvatar(avatarLists[i]);

            if (i != thisPlayerRank - 1)
            {
                newRanking.bar.GetComponent<Image>().color = otherPlayerColor;
            }
            else
            {
                newRanking.bar.GetComponent<Image>().color = thisPlayerColor;
            }
        }

        //if (ifTop5 == false)
        //{
        //    Ranking_Space newRankingSpace = Instantiate(rankingSpacePrefab, transform);
        //    Ranking newRanking = Instantiate(rankingPrefab, transform);
        //    // adjust the bar width, we fix bar to left and use offset from right to change width

        //    float maxWidth = newRanking.barPanel.rect.width;
        //    RectTransform rt = newRanking.bar.GetComponent<RectTransform>();
        //    Debug.Log(normalizedThisPlayerScore);
        //    rt.offsetMax = new Vector2(-(1 - normalizedThisPlayerScore) * maxWidth, rt.offsetMax.y);

        //    newRanking.playerRank.text = thisPlayerRank.ToString();
        //    newRanking.playerName.text = thisPlayerName;
        //    newRanking.bar.GetComponent<Image>().color = thisPlayerColor;

        //}
    }

    List<float> normalizeScores(List<float> originalList)
    {
        List<float> returnList = new List<float>();

        for (int i = 0; i < originalList.Count; i++)
        {
            returnList.Add(originalList[i] / originalList[0]); 
        }
        return returnList;
    }

    public IEnumerator GetScore()
    {
        string resultsURL = "http://localhost:5000/assignmentResults/";
        Debug.Log("Retrieving results");
        UnityWebRequest request = UnityWebRequest.Get(resultsURL);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        // https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
        // If this is a Json array from the server and you did not create it by hand:
        //AssignmentResult x = JsonUtility.FromJson<AssignmentResult>(request.downloadHandler.text.Substring(1, 207));
        //Debug.Log(x);
        //Debug.Log(x._id);
        //Debug.Log(x.score);

        // results is an array of everyone's scores in every single assignment
        string value = "{\"Items\":" + request.downloadHandler.text + "}";
        AssignmentResult[] results = JsonHelper.FromJson<AssignmentResult>(value);
        //Debug.Log(request.downloadHandler.text);
        Debug.Log(request.downloadHandler.text);
        // first we filter the assignment result down to the one that the current player is in

        AssignmentResult[] filtered = results.Where(e => e.assignmentId.Equals(GameIdManager.gameId)).ToArray();
        //AssignmentResult[] filtered = results.Where(e => e.assignmentId.Equals("60602ffe9e0e3e4dc49cc661")).ToArray();

        // This is the number of attempts in the specified assignmentId
        // Note: same player can have multiple attempts
        //Debug.Log(filtered.Length);

        AssignmentResult[] final = filtered.GroupBy(x => x.userId).Select(x => x.OrderByDescending(y => y.score).First()).OrderByDescending(x => x.score).ToArray();

        playerIds = new List<string>();
        playerNames = new List<string>();
        playerScores = new List<float>();
        playerAvatars = new List<int>();
        foreach (AssignmentResult result in final)
        {
            playerIds.Add(result.userId);
            playerNames.Add(result.userName);
            playerScores.Add(float.Parse(result.score));
            playerAvatars.Add(result.avatar);
        }

        // backup
        //Array.Sort(filtered, (left, right) => float.Parse(right.score).CompareTo(float.Parse(left.score)));

        //playerNames = new List<string>();
        //playerScores = new List<float>();
        //foreach (AssignmentResult result in filtered)
        //{
        //    playerNames.Add(result.userId);
        //    playerScores.Add(float.Parse(result.score));
        //}


        // normalizedScores are between 0 to 1
        List<float> normalizedScores = normalizeScores(playerScores);

        thisPlayerRank = getMyRanking() + 1;

        chartWidth = Screen.width + GetComponent<RectTransform>().sizeDelta.x;

        // Display rankings
        DisplayRanking(playerNames, normalizedScores, playerAvatars);
    }

    [Serializable]
    class AssignmentResult
    {
        public List<string> wrongQuestionIds;
        public string _id;
        public string userId;

        public string userName;

        public int avatar;
        public string assignmentId;
        public string score;
        public float __v;

        public AssignmentResult(List<string> wrongQuestionIds, string id, string userId, string assignmentId, string score, float v)
        {
            this.wrongQuestionIds = wrongQuestionIds;
            this._id = id;
            this.userId = userId;
            this.assignmentId = assignmentId;
            this.score = score;
            this.__v = v;
        }
    }

    public int getMyRanking()
    {
        foreach (int i in Enumerable.Range(0, playerIds.Count))
        {
            if (playerIds[i].Equals(PlayerLevel.userId))
            {
                return i;
            }
        }
        return -1; ;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
