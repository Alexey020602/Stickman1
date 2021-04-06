using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Progress
{
    public static float TheBestCounter = 0;

    public static bool[] Scenes = new bool[10]{true, false, false, false, false, false, false, false, false, false};
}

public class GameManager : SimpleSingleton<GameManager>
{
    public Button[] Buttons = new Button[0];
    private PlayerManager _player;
    public bool FreezeOnStart = true;
    private CanvasManager Canvas;

    public GameObject ObstacleMenuButton;
    public Transform Obstacles;
    public Transform Props;
    private string nameOfScene;
    [System.Serializable]
    public class ObstacleInventory
    {
        public string ObstacleName;
        public GameObject ObstaclePrefab;
        //[HideInInspector]
        //public GameObject Obstacle;
    }
    public List<ObstacleInventory> ListOfObstacles=new List<ObstacleInventory>();

    public float CurrentCounter;
    public static float TheBestCounter;


    public Transform _arrowDirection;

    
    void Start()
    {
        _player = PlayerManager.Instance;
        Canvas = CanvasManager.Instance;

        TheBestCounter = Progress.TheBestCounter;
        Canvas.UpdateTheBestCounter(TheBestCounter);
        WaitForStartGame();
        StartButtons();
        nameOfScene = SceneManager.GetActiveScene().name;
        SetObstacles();
        

    }
    public void SetObstacles()
    {

        for (int i = 0; i < Props.childCount; i++)
        {
            Transform PositionOfObstacle = Props.GetChild(i);
            string name = PlayerPrefs.GetString($"{nameOfScene}type{i}");
            foreach (ObstacleInventory obstacle in ListOfObstacles)
            {
                if (obstacle.ObstacleName == name)
                {
                    OpenPropsMenu installationPoint = PositionOfObstacle.gameObject.GetComponent<OpenPropsMenu>();
                    if (installationPoint.IsUsed())
                    {
                        Destroy(installationPoint.AttachedObstacle());
                    }
                    installationPoint.AttachMountedObstacle(Instantiate<GameObject>(obstacle.ObstaclePrefab, PositionOfObstacle.position + Vector3.forward, obstacle.ObstaclePrefab.transform.rotation, Obstacles), name);
                    installationPoint.Used();
                }
            }
        }
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


    //мой кодик
    public void StartGame(float Force, Vector3 direction)
    {
        //Vector3 direction = new Vector3(Mathf.Cos((_arrowDirection.rotation.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Sin((_arrowDirection.rotation.eulerAngles.z) * Mathf.Deg2Rad)); ;
        if (_player)
        {
            _player.SetImpulse(Force, -direction);
        }
        ObstacleMenuButton.SetActive(false);
      //  Debug.LogWarning(direction);
        
        Time.timeScale = 1f;
    }
    //мой кодик гг

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
    private void OnApplicationQuit()
    {
        
        for (int i=0; i<Props.childCount; i++)
        {
            
            Transform obstacle = Props.GetChild(i);
            OpenPropsMenu obstaclePlace = obstacle.GetComponent<OpenPropsMenu>();
            PlayerPrefs.SetString($"{nameOfScene}type{i}", obstaclePlace.TypeAttachedObstacle());
        }
        PlayerPrefs.Save();
    }
    
}
