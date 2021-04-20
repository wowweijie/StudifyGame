using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetnReturn : MonoBehaviour
{

    public string returnSceneName;

    public void returnMenu()
    {
        resetVariable();
        SceneManager.LoadScene(returnSceneName);
    }

    public void resetVariable()
    {
        PlayerLevel.level = 1;
        PlayerLevel.score = 0;
        PlayerLevel.wrongQuestionIDs = new List<string>();
        PlayerLevel.upgrade = false;
        PlayerLevel.category = "All";
        PlayerLevel.currentDifficultyIndex = 0;
    }

    // Start is called before the first frame update
    //void Start()
    //{


    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
