using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacebookManager : MonoBehaviour
{
    [SerializeField] Text inputGameCode;
    private string gameCode, link, caption, shareLink;

    public void gameCodeShare()
    {
        gameCode = inputGameCode.text;
        link = "https://youtu.be/i-QyW8D3ei0";
        caption = "Your friend has challenged you to a game in Studify! Enter Game Code < " + gameCode + " > to fight now!";
        shareLink = "https://www.facebook.com/sharer/sharer.php?u=" + link + "&quote=" + caption;
        Application.OpenURL(shareLink);
    }
}
