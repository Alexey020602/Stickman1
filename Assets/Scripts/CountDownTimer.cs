using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CountDownTimer : MonoBehaviour
{
    private GameManager _gamemanager;
    public float timeLeft = 0f;
    public Text startText;
    public UnityEvent OnLeftTime;
    public bool ActiveCounter = false;

    private void Start()
    {
        _gamemanager = GameManager.Instance;
        startText = gameObject.GetComponent<Text>();
    }



    void Update()
    {
        if (ActiveCounter)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 3.5f)
            {
                startText.enabled = true;
                startText.text = (timeLeft).ToString("0");
                if (timeLeft < 0)
                {
                    ActiveCounter = false;
                    startText.enabled = false;
                    _gamemanager.EndGameMenu(true);
                    OnLeftTime.Invoke();

                }
            }
        }
        else
        {
            startText.enabled = false;
        }
    }
}
