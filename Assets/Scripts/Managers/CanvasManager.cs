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
    private void Update()
    {
        StaminaBar.fillAmount = Player.stamina;
    }



}
