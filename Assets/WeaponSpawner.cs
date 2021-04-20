using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    [Tooltip("Each Level (except Level 1) 's Reward")]
    public List<String> LevelRewards = new List<String>();

    [Tooltip("Drop Location")]
    public float xPos;
    public float yPos;
    public float zPos;

    private PickupFactory m_PickupFactory;
    void Start()
    {
        m_PickupFactory = FindObjectOfType<PickupFactory>();
        if (PlayerLevel.upgrade == true)
        {
            // -2 because -1 for starting from index 0, another -1 because no spawn during level 1
            String gameObjectString = LevelRewards[PlayerLevel.level - 2];
            GameObject gameObject = m_PickupFactory.getPickupInterface(gameObjectString);
            Instantiate(gameObject, new Vector3(xPos, yPos, zPos), Quaternion.identity);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public static WeaponSpawner _instance;

    public static WeaponSpawner Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<WeaponSpawner>();
            }

            return _instance;
        }
    }

}
