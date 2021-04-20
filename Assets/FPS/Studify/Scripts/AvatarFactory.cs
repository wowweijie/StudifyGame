using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AvatarFactory : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Avatar Sprites")]
    public List<Sprite> avatars;

    public static List<Sprite> avatarSprites;

    public Sprite getAvatar(int avatarIndex){
        if (avatarIndex < avatars.Count) {
            return avatars[avatarIndex];
        }
        
        Debug.Log($"Index out of range. Only {avatars.Count} avatars");
        return null;
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

    public static AvatarFactory _instance;

    public static AvatarFactory Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AvatarFactory>();
            }

            return _instance;
        }
    }


}
