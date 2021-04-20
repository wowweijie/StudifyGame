using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickupFactory : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Pickup Prefabs")]
    public GameObject launcher;
    public GameObject shotgun;
    public GameObject blaster;
    public GameObject health;    

    public GameObject getPickupInterface(string pickupName){
        switch(pickupName){
            case "launcher" :
                return launcher;
            case "shotgun" : 
                return shotgun;
            case "blaster" :
                return blaster;
            case "health" :
                return health;
            default:
                Debug.Log("Invalid Pickup Name. No object generated");
                return null;
        }
    }

    public WeaponController getWeaponController(string weaponName){
        switch(weaponName){
            case "launcher" :
                return launcher.GetComponent<WeaponPickup>().weaponPrefab;
            case "shotgun" : 
                return shotgun.GetComponent<WeaponPickup>().weaponPrefab;
            case "blaster" :
                return blaster.GetComponent<WeaponPickup>().weaponPrefab;
            default:
                Debug.Log("Invalid Weanpon Name. No object generated");
                return null;
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public static PickupFactory _instance;

    public static PickupFactory Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PickupFactory>();
            }

            return _instance;
        }
    }


}
