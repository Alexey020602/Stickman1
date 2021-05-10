using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : SimpleSingleton<CanvasManager>
{
    [Header("Counters")]
    public Text CurrentCounter;
    public Text TheBestCounter;
    public CountDownTimer CountDownTimer;


    [Header("Player")]
    public Image StaminaBar;
    public PlayerManager Player;

    [Header("Menu")]
    public GameObject _RestartMenu;
    public GameObject _PauseMenu;
    public GameObject _TransportMenu;
    public GameObject _ObstaclesMenu;


    [Header("Restart Menu")]
    public Text RMTheBestCounter;


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
        RMTheBestCounter.text = "Лучший счет: " + (int)GameManager.Instance.TheBestCounter;
    }
    public void PauseMenu(bool active)
    {
        _PauseMenu.SetActive(active);
    }
    public void TransportMenu(bool active)
    {
        _TransportMenu.SetActive(active);
    }

    private void Update()
    {
        StaminaBar.fillAmount = Player.stamina;
    }



}
