using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameIdManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static string gameId;

    public InputField gameIdField;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setGameId(){
        gameId = gameIdField.text;
    }
}
