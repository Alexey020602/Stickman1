using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class Levelmanager : SimpleSingleton<Levelmanager>
{
    public Image StaminaBar;
    public Text CurrentScoreText;

    [Header("Menus")]
    public EndGameMenu EndGameMenu;
    public DisplayedUIPanel PauseMenu;
    public SelectTransport SelectTransportMenu;

    [Header("Buttons")]
    public GameObject StickmanControlButtons;
    public GameObject TransportControlButtons;
    public GameObject StartButton;
    public GameObject SimpleStartButton;

    [Header("Other")]
    public UnityEvent OnStartGame;
    public MotorTransport CurrentTransport;



    private int currentScore;
    private int theBestScore
    {
        get
        {
            return PlayerPrefs.GetInt("TheBestScore--");
        }
        set
        {
            PlayerPrefs.SetInt("TheBestScore--", value);
            PlayerPrefs.Save();
        }
    }
    private PlayerManager.ControlTypes controlType 
    {
        get
        {
            return PlayerManager.Instance.ControlType;
        }
    }
    private float timeScaleBeforePause;


    public void UpdateStamina(float value)
    {
        StaminaBar.fillAmount = value;
    }

    public void UpdateCurrentScore(int val)
    {
        currentScore += val;

        CurrentScoreText.text = currentScore.ToString();
    }

    public void StartButton_SetActive(bool state)
    {
        StartButton.SetActive(state);
    }

    public void StartGame(bool transportControl)
    {
        Pause(false);
        OnStartGame?.Invoke();

        if (controlType == PlayerManager.ControlTypes.Buttons)
            StickmanControlButtons.SetActive(true);

        EnableTransportControl(transportControl);
    }

    public void SelectTransport(string name)
    {
        bool state = SelectTransportMenu.Select(name);

        if(PlayerManager.Instance.StartGameType != PlayerManager.StartGameTypes.ButtonWithoutPush)
            SimpleStartButton.SetActive(state);

        if(PlayerManager.Instance.StartGameType == PlayerManager.StartGameTypes.ButtonWithPush)
            StartButton.SetActive(!state);

    }

    #region StickmanControl

    public void SetStickmanPose(string poseName)
    {
        PlayerManager.Instance.SetControlOfBodyPart(poseName);
    }
    public void DisableStickmanPose()
    {
        PlayerManager.Instance.DisabledControlOfBodyPart();
    }

    public void Cath()
    {
        PlayerManager.Instance.Catch();
    }
    public void UnCatch()
    {
        PlayerManager.Instance.UnCatch();
    }

    public void EnableTransportControl(bool state, MotorTransport transport = null)
    {
        if(transport)
            CurrentTransport = transport;

        if (controlType == PlayerManager.ControlTypes.Buttons)
            StickmanControlButtons.SetActive(!state);
        if (controlType == PlayerManager.ControlTypes.Buttons)
            TransportControlButtons.SetActive(state);

        PlayerManager.Instance.SetInfinityStamina(state);
        PlayerManager.Instance.SetStickmanControl(!state);
    }

    public void EnableStickmanControl(bool state)
    {
        if (controlType == PlayerManager.ControlTypes.Buttons)
            StickmanControlButtons.SetActive(state);
        if (controlType == PlayerManager.ControlTypes.Buttons)
            TransportControlButtons.SetActive(!state);

        PlayerManager.Instance.SetInfinityStamina(!state);
        PlayerManager.Instance.SetStickmanControl(state);
    }

    #endregion

    #region TransportControl

    public void SetTransportHorizontalDirection(float direction)
    {
        CurrentTransport.SetHorizontalControl(direction);
    }

    public void SetTransportRotate(float direction)
    {
        CurrentTransport.SetAngularControl(direction);
    }
    #endregion

    #region Menu Control

    public void EndGame()
    {
        if(currentScore > theBestScore)
        {
            theBestScore = currentScore;
        }

        EndGameMenu.OpenPanel(currentScore.ToString(), theBestScore.ToString(), "0", "0");
    }

    public void OpenPauseMenu()
    {
        timeScaleBeforePause = Time.timeScale;
        Pause(true);
        PauseMenu.OpenPanel();
    }
    public void ClosePauseMenu()
    {
        Pause(timeScaleBeforePause);
        PauseMenu.ClosePanel();
    }

    public void OpenSelectTransportMenu()
    {
        SelectTransportMenu.OpenPanel();
    }
    public void CloseSelectTransportMenu()
    {
        SelectTransportMenu.ClosePanel();
    }

    #endregion

    #region TimeDilation

    private Coroutine startingTimeDilation_Cor;
    private Coroutine durationTimeDilation_Cor;
    private Coroutine endingTimeDilation_Cor;
    private float changeSpeedTimeDilation;

    public void StartDilation(float duration, float timeScale)
    {
        // если мы в стадии начинания замедления времени, то ничего не делаем
        if (startingTimeDilation_Cor != null)
            return;
        // если у нас время уже в замедлении, то обновляем таймер
        if (durationTimeDilation_Cor != null)
        {
            StopCoroutine(durationTimeDilation_Cor);
            StartCoroutine(DurationTimeDilationIE(duration));
            return;
        }
        // если мы в стадии заканчивавния замедления времени, то просто осталавливаем этот корутин и начинаем сначала
        if (endingTimeDilation_Cor != null)
            StopCoroutine(endingTimeDilation_Cor);

        startingTimeDilation_Cor = StartCoroutine(StartingTimeDilationIE(duration, timeScale));
    }

    IEnumerator StartingTimeDilationIE(float duration, float timeScale)
    {
        float lastTimeScale = Time.timeScale;
        for (float i = 0f; i < 1f; i += Time.deltaTime * 5f)
        {
            Time.timeScale = Mathf.Lerp(lastTimeScale, timeScale, i);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return null;
        }
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        durationTimeDilation_Cor = StartCoroutine(DurationTimeDilationIE(duration));
        startingTimeDilation_Cor = null;
    }

    IEnumerator DurationTimeDilationIE(float duration)
    {
        yield return new WaitForSeconds(duration);
        endingTimeDilation_Cor = StartCoroutine(EndingTimeDilationIE());
        durationTimeDilation_Cor = null;
    }

    IEnumerator EndingTimeDilationIE()
    {
        float lastTimeScale = Time.timeScale;
        for (float i = 0f; i < 1f; i += Time.deltaTime * 3f)
        {
            Time.timeScale = Mathf.Lerp(lastTimeScale, 1f, i);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return null;
        }
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        endingTimeDilation_Cor = null;
    }
    #endregion

    #region System

    public void Pause(float scale)
    {
        Time.timeScale = scale;
    }

    public void Pause(bool state)
    {
        if(state)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    #endregion

}
