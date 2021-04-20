using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    [SerializeField] public Button _button;
    [SerializeField] public Text _buttonText;
    [SerializeField] public Text _popupText;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Transform canvas)
    {
        transform.SetParent(canvas);
        transform.localScale = Vector3.one;


        GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;

        transform.Find("Panel").Find("Button").localScale = new Vector2(2,2);
        Transform popupTextTransform =  transform.Find("Panel").Find("Text");
        popupTextTransform.localScale = new Vector2(2,2);
        popupTextTransform.localPosition = new Vector3(0, 160, 0);
        RectTransform popupRectTransform = popupTextTransform.GetComponent<RectTransform>();
        popupRectTransform.offsetMin = new Vector2(-25, popupRectTransform.offsetMin.y);
        popupRectTransform.offsetMax = new Vector2(35, popupRectTransform.offsetMax.y);

        _button.onClick.AddListener(() => {
            GameObject.Destroy(this.gameObject);
        }); 
    }

    
}
