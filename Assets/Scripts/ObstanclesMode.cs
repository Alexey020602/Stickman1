using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstanclesMode : MonoBehaviour
{
    public GameObject Props;
    public GameObject PlayButtonsAndIndicators;
    public CametaFollow Camera;

    private Text text;
    private bool isModeOn=false;
    // Start is called before the first frame update
    void Start()
    {
        text= transform.GetChild(0).gameObject.GetComponent<Text>();
    }
    public void ClickOnButtonPropMenu()
    {
        if(isModeOn)
        {
            text.text = "Menu location";
            isModeOn = false;
            PlayButtonsAndIndicators.SetActive(true);
            Props.SetActive(false);
        }
        else
        {
            text.text = "Return to Stickman";
            isModeOn = true;
            PlayButtonsAndIndicators.SetActive(false);
            Props.SetActive(true);
        }
        Camera.MovingCamera();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
