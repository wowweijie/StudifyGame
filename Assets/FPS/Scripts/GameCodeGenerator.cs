using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCodeGenerator : MonoBehaviour
{
    [SerializeField] Text gameCode;
    // Start is called before the first frame update
    void Start()
    {
        gameCode.text = CreateManager.createdGameId;
    }
}
