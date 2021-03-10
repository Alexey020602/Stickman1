using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private GameManager _gamemanager;
    private Image _image;


    private void Start()
    {
        _gamemanager = GameManager.Instance;
        _image = gameObject.GetComponent<Image>();
    }

    public void PressStartButton()
    {
        //_gamemanager.StartGame(_image.fillAmount);

    }



}
