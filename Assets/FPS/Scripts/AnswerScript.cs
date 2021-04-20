using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    // false by default
    private QuizManager quizMananger;
    
    void Start(){
        quizMananger = GameObject.Find("QuizManager").GetComponent<QuizManager>();
    }

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer.");
            quizMananger.correct();
        }
        else
        {
            Debug.Log("Wrong Answer.");
            quizMananger.wrong();
            // call correct method anyways to go to the next question
        }
    }
}
