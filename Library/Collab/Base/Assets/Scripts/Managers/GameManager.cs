using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Progress
{
    public static float TheBestCounter = 0;

    public static bool[] Scenes = new bool[10]{true, false, false, false, false, false, false, false, false, false};
}

public class GameManager : MonoBehaviour
{
    public Button[] Buttons = new Button[0];
    private PlayerManager _player;
    public bool FreezeOnStart = true;
    private CanvasManager Canvas;


    public float CurrentCounter;
    public static float TheBestCounter;


    
    void Start()
    {
        try { _player = GameObject.Find("Stickman").GetComponent<PlayerManager>(); } catch { Debug.Log("No Player at the scene"); }
        try { Canvas = GameObject.Find("Canvas").GetComponent<CanvasManager>(); } catch { Debug.Log("No Canvas at the scene"); }
        TheBestCounter = Progress.TheBestCounter;
        Canvas.UpdateTheBestCounter(TheBestCounter);
        WaitForStartGame();
        StartButtons();
    }
    void StartButtons()
    {

        for (int i = 0; Buttons.Length > i; i++)
        {
            if (Buttons[i] == null)
            {
                continue;
            }
            bool a = Progress.Scenes[i];
            if (a)
            {
                Buttons[i].interactable = true;
            }
            else
            {
                Buttons[i].interactable = false;
            }
        }
    }
    public void UpdateCurrentCounter(float Damage)
    {
        CurrentCounter += Damage;
        Canvas.UpdateCurrentCounter(CurrentCounter);
    }


    // =========================================состояния игры=========================================
    public void StartGame(float Force)
    {
        if (_player)
            _player.SetImpulse(Force);
        Time.timeScale = 1f;
    }


    public void WaitForStartGame()
    {
        Time.timeScale = 0f;
    }



    public void Pause(bool state)
    {
        if (state)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        if (Canvas)
            Canvas.PauseMenu(state);


    }


    public void EndGameMenu(bool state)
    {
        if (CurrentCounter > TheBestCounter)
        {
            TheBestCounter = CurrentCounter;
            Progress.TheBestCounter = TheBestCounter;
            if (Canvas)
                Canvas.UpdateTheBestCounter(TheBestCounter);
        }
        {
            if (Canvas)
                Canvas.RestartMenu(state);
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {

    }


    public void SelectiLevel()
    {

    }

    public void LoadScene(string Level)
    {
        SceneManager.LoadScene(Level);
    }
    // =========================================состояния игры=========================================

}
