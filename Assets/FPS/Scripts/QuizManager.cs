using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class QuizManager : MonoBehaviour
{
    public GameObject AnswerButton;
    public List<QuestionAndAnswers> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public GameObject Quiz_Panel;

    public Transform AnswerPanel;
    public GameObject GameOver_Panel;

    public TextMeshProUGUI QuestionTxt;

    public TextMeshProUGUI DifficultyTxt;
    public TextMeshProUGUI ScoreTxt;

    int totalQuestions = 0;
    public int score;

    string Next_Scene;

    private string currentQuestionId;
    private Renderer rend;
    private Color buttonColor = Color.white;

    private void Start()
    {
        totalQuestions = QnA.Count;
        GameOver_Panel.SetActive(false);
        Debug.Log($"Player Mode : {PlayerLevel.category}");
        if(PlayerLevel.category=="All"){
            StartCoroutine(GetQuestion(this.setQuestionText, this.setOption));
            generateQuestion();
        }
        else{
            StartCoroutine(GetCampaignQuestion(this.setQuestionText, this.setOption));
        }
    }

    IEnumerator GetQuestion(Action<string, string> questionCallback, Action<String[], String> optionCallback){
        
        string retrieveQuestion = "http://localhost:5000/assignments/question/" + 
         GameIdManager.gameId + 
        "?level=" + PlayerLevel.level.ToString();
        
        Debug.Log("sending api: " + retrieveQuestion);
        UnityWebRequest questionRequest = UnityWebRequest.Get(retrieveQuestion);
        questionRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();

        yield return questionRequest.SendWebRequest();

        if(questionRequest.responseCode==200){
            Debug.Log($"Question Id : {questionRequest.downloadHandler.text}");
            currentQuestionId = questionRequest.downloadHandler.text;
            StartCoroutine(GetQuestionFromId(currentQuestionId, questionCallback, optionCallback));
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

    IEnumerator GetQuestionFromId(string questionId, Action<string, string> questionCallback, Action<String[], String> optionCallback){
        
        string retrieveQuestion = "http://localhost:5000/questions/" + questionId;
        Debug.Log("sending api: " + retrieveQuestion);
        UnityWebRequest request = UnityWebRequest.Get(retrieveQuestion);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if(request.responseCode==200){
            Debug.Log(request.downloadHandler.text);

            Question questionObject = JsonUtility.FromJson<Question>(request.downloadHandler.text);
            Debug.Log(questionObject.question);
            questionCallback(questionObject.question, questionObject.difficulty);
            optionCallback(questionObject.options, questionObject.answer);
        }

        else if (request.responseCode==401){
            Debug.Log("Fail to retrieve question");
        }

        else if (request.responseCode==500){
            Debug.Log(request.downloadHandler.text);
        }

        if (request.isNetworkError || request.isHttpError) {
            Debug.LogError(request.error);
            yield break;
        }

        
    }

    IEnumerator GetCampaignQuestion(Action<string, string> questionCallback, Action<String[], String> optionCallback){

        CampaignQueryBody jsonBodyObject = new CampaignQueryBody();
        jsonBodyObject.category = PlayerLevel.category;
        jsonBodyObject.difficulty = difficultyModes[PlayerLevel.currentDifficultyIndex];
        jsonBodyObject.pastQnsIds = PlayerLevel.wrongQuestionIDs;

        Debug.Log($"jsonbody: {JsonUtility.ToJson(jsonBodyObject, true)}");
        string jsonBody = JsonUtility.ToJson(jsonBodyObject);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        
        string campaignQuery = "http://localhost:5000/questions/gamequery";
        
        Debug.Log("sending api: " + campaignQuery);
        Debug.Log("body: " + jsonBody);
        UnityWebRequest questionRequest = new UnityWebRequest(campaignQuery, "POST");
        questionRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        questionRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        questionRequest.SetRequestHeader("Content-Type", "application/json");

        yield return questionRequest.SendWebRequest();

        if(questionRequest.responseCode==200){
            Debug.Log($"response: {questionRequest.downloadHandler.text}"); 
            Question questionObject = JsonUtility.FromJson<Question>(questionRequest.downloadHandler.text);
            currentQuestionId = questionObject._id;
            Debug.Log(questionObject.question);
            questionCallback(questionObject.question, questionObject.difficulty);
            optionCallback(questionObject.options, questionObject.answer);
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

    public void correct()
    {
        score += 1;
        changeDifficulty(true);
        GameOver();
    }

    public void wrong()
    {
        PlayerLevel.wrongQuestionIDs.Add(currentQuestionId);
        changeDifficulty(false);
        GameOver();
    }

    void setQuestionText(string qnsText, string difficulty){
        Debug.Log("Set qns and difficulty");
        Debug.Log(qnsText);
        Debug.Log(difficulty);
        QuestionTxt.text=qnsText;
        DifficultyTxt.text=difficulty;
    }

    void setOption(string[] options, string answer){
        foreach (string optionText in options){
            GameObject optionButton = Instantiate(AnswerButton, new Vector3(0,0,0), Quaternion.identity);
            optionButton.transform.SetParent(AnswerPanel);
            optionButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text=optionText;
            optionButton.GetComponent<AnswerScript>().isCorrect=false;
            if(optionText.Equals(answer)){
                optionButton.GetComponent<AnswerScript>().isCorrect=true;
            }

        }
    }

    void generateQuestion()
    {
        if (QnA.Count > 0)
        {
            currentQuestion = UnityEngine.Random.Range(0, QnA.Count);
            QuestionTxt.text = QnA[currentQuestion].Question;
            //generateAnswers();
        }
        else
        {
            Debug.Log("No more question.");
            GameOver();
        }
    } 

    void GameOver()
    {
        PlayerLevel.score += score; 
        // Disable the quiz panel and enable the gameover panel
        Quiz_Panel.SetActive(false);
        GameOver_Panel.SetActive(true);
        ScoreTxt.text = "Current score : " + PlayerLevel.score;


        // If get full mark during the quiz, then the upgrade for the next scene will spawn
        if (score != 0)
        {
            PlayerLevel.upgrade = true;
            string rewardName = WeaponSpawner.Instance.LevelRewards[PlayerLevel.level-1];
            GameObject rewardIcon = PickupFactory.Instance.getPickupInterface(rewardName);
            GameObject rewardIconClone = Instantiate(rewardIcon, new Vector3(-60, 60,-100), Quaternion.identity);
            rewardIconClone.transform.localScale = new Vector3(150, 150, 150);
        }
        else
        {
            PlayerLevel.upgrade = false;
        }
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void next()
    {
        // send results to database 
        Debug.Log($"player level: {PlayerLevel.level}");
        if (PlayerLevel.level>=5 && PlayerLevel.category == "All"){
            StartCoroutine(SendResults());
            SceneManager.LoadScene("LeaderBoard_Test");
        }
        else{
            if (PlayerLevel.level>=5){
                SceneManager.LoadScene("EndCampaign");
            }else{
                //StartCoroutine(SendResults()); // sending every level
                PlayerLevel.level += 1;
                SceneManager.LoadScene("MainScene");
            }
        }
        // Also can put SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // Can see and change scene number under File > Build Settings
    }

    IEnumerator SendResults(){

        Debug.Log("sending results...");
        //Debug.Log(PlayerLevel.userId);
        //Debug.Log(PlayerLevel.userName);
        //Debug.Log(PlayerLevel.avatar);
        //Debug.Log(GameIdManager.gameId);
        //Debug.Log(PlayerLevel.score);
        //Debug.Log(PlayerLevel.wrongQuestionIDs);

        AssignmentResult resultObj = new AssignmentResult(
            PlayerLevel.userId,
            PlayerLevel.userName,
            PlayerLevel.avatar,
            GameIdManager.gameId,
            PlayerLevel.score,
            PlayerLevel.wrongQuestionIDs
        );

        string jsonBody = JsonUtility.ToJson(resultObj);

        Debug.Log(jsonBody); // correct

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        
        
        string createResults = "http://localhost:5000/assignmentResults/";
        
        UnityWebRequest resultsRequest = new UnityWebRequest(createResults, "POST");
        resultsRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        resultsRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        resultsRequest.SetRequestHeader("Content-Type", "application/json");

        Debug.Log(bodyRaw);
        Debug.Log(jsonBody);

        yield return resultsRequest.SendWebRequest();

        if(resultsRequest.responseCode==201){
            Debug.Log($"Results Saved : {resultsRequest.downloadHandler.text}");
        }

        else if (resultsRequest.responseCode==400){
            Debug.Log("Failed to save results");
        }

        else if (resultsRequest.responseCode==500){
            Debug.Log(resultsRequest.downloadHandler.text);
        }

        if (resultsRequest.isNetworkError || resultsRequest.isHttpError) {
            Debug.LogError(resultsRequest.error);
            yield break;
        }

        
    }

    class AssignmentResult{

        public AssignmentResult(string userId, string userName,int avatar, string assignmentId, int score, List<string> wrongQuestionIds)
        {
            this.userId = userId;
            this.userName = userName; // missing
            this.avatar = avatar; // missing
            this.assignmentId = assignmentId;
            this.score = score;
            this.wrongQuestionIds = wrongQuestionIds;
        }

        public string userId;

        public string userName;

        public int avatar;
        public string assignmentId;

        public int score;

        public List<string> wrongQuestionIds;
    }

    class CampaignQueryBody{
        public string category;

        public string difficulty;

        public List<string> pastQnsIds;

    }

    string[] difficultyModes = {"Easy", "Medium", "Hard"}; 

    void changeDifficulty(bool increase){
        int currentValue = PlayerLevel.currentDifficultyIndex;
        if (increase){
            currentValue+=1;
        }else{
            currentValue-=1;
        }

        if (currentValue < 0){
            currentValue = 0;
        }else if (currentValue > 2) {
            currentValue = 2;
        }

        PlayerLevel.currentDifficultyIndex = currentValue;
    }
    
}
