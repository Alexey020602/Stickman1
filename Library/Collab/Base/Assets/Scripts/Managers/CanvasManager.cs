using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    public Text CurrentCounter;
    public Text TheBestCounter;

    public GameObject _RestartMenu;
    public GameObject _PauseMenu;

    private Animator AnimatorToEndGame;
    void Start()
    {
        //CurrentCounter = GameObject.Find("CurrentCounter").GetComponent<Text>();
       // TheBestCounter = GameObject.Find("TheBestCounter").GetComponent<Text>();
    }

    public void UpdateCurrentCounter(float Counter)
    {
        CurrentCounter.text = "Счет:" + (int)Counter;
    }
    public void UpdateTheBestCounter(float counter)
    {
        TheBestCounter.text = "Лучший счет:" + (int)counter;
    }

    public void RestartMenu(bool active)
    {
        _RestartMenu.SetActive(active);
    }
    public void PauseMenu(bool active)
    {
        _PauseMenu.SetActive(active);
    }




}
