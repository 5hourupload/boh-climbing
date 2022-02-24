using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingbar : MonoBehaviour {

    private RectTransform rectComponent;
    public Image imageComp;
    public float speed = 0.0f;
    public bool grab = true; 

    void Awake(){
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = 1.0f;
    }
   

    // Use this for initialization
    void Start () {
    
    }

    void Update()
    {
        if (imageComp.fillAmount == 0f)
        {
            grab = false;
            
        }

    }

    public void addValue()
    {
        imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;
    }

    public void minusValue()
    {
        imageComp.fillAmount = imageComp.fillAmount - Time.deltaTime * speed;
        if (imageComp.fillAmount < 0.3){
            imageComp.color = new Color32(255,0,0,255);
        }
    }
}
