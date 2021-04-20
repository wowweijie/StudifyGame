using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Dropdown categoryDropdown;

    public GameObject popupPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCategory(){
        PlayerLevel.category = categoryDropdown.options[categoryDropdown.value].text;
        Debug.Log($"PlayerLevel category set to {PlayerLevel.category}");

        if(categoryDropdown.value==0)
        {
            GameObject popupMsg = Instantiate(popupPrefab);
            popupMsg.GetComponent<PopupManager>().Init(GameObject.Find("Canvas").transform);
        }
        else {
            SceneManager.LoadScene("CampaignVideo");
        }
    }
}
