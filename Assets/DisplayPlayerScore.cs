using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPlayerScore : MonoBehaviour
{
    public TextMeshProUGUI PlayerScoreDisplay;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerLevel.score);
        PlayerScoreDisplay.SetText("Latest Score: " + PlayerLevel.score.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
