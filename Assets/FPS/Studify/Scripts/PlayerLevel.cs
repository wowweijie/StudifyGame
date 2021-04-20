using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel 
{
    static public int level = 1;

    static public string userName;

    static public int avatar;

    static public string userId;
    //static public string userId = "6038b009801a6c272c853712";

    static public int score = 0;

    static public List<string> wrongQuestionIDs = new List<string>();

    // This decides whether the item will be spawned
    static public bool upgrade = false;
    
    static public string category = "All";

    static public int currentDifficultyIndex = 0;

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
