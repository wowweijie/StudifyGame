using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TwitterManager : MonoBehaviour
{
    [SerializeField] Text inputGameCode;
    
    private string twitterAddress = "http://twitter.com/intent/tweet";
    private string displayMessage, gameCode;

    [System.Obsolete]
    public void shareGameCodeToTwitter()
    {
        gameCode = inputGameCode.text;
        displayMessage = "Enter Game Code <" + gameCode + "> to fight now!";
        Application.OpenURL(twitterAddress + "?text=" + WWW.EscapeURL("Your friend has challenged you to a game in Studify!\n" + displayMessage));
    }
}
