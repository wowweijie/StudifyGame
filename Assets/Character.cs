using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    private GameObject[] characterList;
    private int index;

    // Start is called before the first frame update
    private void Start()
    {
        index = PlayerPrefs.GetInt("CharacterSelected");
        characterList = new GameObject[transform.childCount];

        // Fill the array with our models
        for (int i = 0; i < transform.childCount; i++)
            characterList[i] = transform.GetChild(i).gameObject;

        // We toggle off the renderer
        foreach (GameObject go in characterList)
            go.SetActive(false);

        // We toggle on the selected character
        if (characterList[index])
            characterList[index].SetActive(true);
    }

    public void ToggleLeft()
    {
        // Toggle off the current model
        characterList[index].SetActive(false);

        index -= 1;

        if (index < 0)
            index = characterList.Length - 1;

        // Toggle on the current model
        characterList[index].SetActive(true);
    }

    public void ToggleRight()
    {
        // Toggle off the current model
        characterList[index].SetActive(false);

        index += 1;

        if (index == characterList.Length)
            index = 0;

        // Toggle on the current model
        characterList[index].SetActive(true);
    }

    public void ConfirmButton()
    {
        Debug.Log("user selected avatar: " + index);
        PlayerPrefs.SetInt("CharacterSelected", index);
        PlayerLevel.avatar = index; 
        SceneManager.LoadScene("MainScene");

    }


}
