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


    public float CurrentCounter;
    public float TheBestCounter;
    public int StarsCounter=0;

    public Transform Stars;
    public GameObject SrarPrefab;


    public GameObject ObstacleMenuButton;
    public Transform Obstacles;
    public Transform Props;
    private ListOfObstaclesToAdd obstaclesList;
    private string nameOfScene;


    [System.Serializable]
    public class ObstacleInventory
    {
        public string ObstacleName;
        public GameObject ObstaclePrefab;
        //[HideInInspector]
        //public GameObject Obstacle;
    }

    public List<ObstacleInventory> ListOfObstacles = new List<ObstacleInventory>();

    void Start()
    {
        obstaclesList = ListOfObstaclesToAdd.Instance;
        _player = PlayerManager.Instance;
        Canvas = CanvasManager.Instance;


        if (PlayerPrefs.HasKey("TheBestCounter"))
        {
            TheBestCounter = PlayerPrefs.GetInt("TheBestCounter");
        }
        else TheBestCounter = 0;

        Canvas.UpdateTheBestCounter(TheBestCounter);
        WaitForStartGame();
        StartButtons();

        nameOfScene = SceneManager.GetActiveScene().name;
        if (nameOfScene == null)
        {
            Debug.LogError("Scene has not name");
            return;
        }
        if (nameOfScene == "MainMenu")
        {
            return;
        }
        SetObstacles();

    }
    //public void SetStars()
    //{
    //    if (Stars == null) return;
    //    for(int i=0; i<Stars.childCount;i++)
    //    {

    //        if( PlayerPrefs.HasKey($"{nameOfScene} {i} star collected"))
    //        {
    //            if(PlayerPrefs.GetInt($"{nameOfScene} {i} star collected")==1)
    //            {
    //                Destroy(Stars.GetChild(i).gameObject);
    //            }
    //        }
    //        else
    //        {
    //            PlayerPrefs.SetInt($"{nameOfScene} {i} star collected", 0);
    //        }
    //    }
    //}
    public void SetObstacles()
    {
        if (obstaclesList == null)
        {
            return;
        }
        //if (obstaclesList.ObstaclesOfScene == null) return;

        if (obstaclesList.ObstaclesOfScene.Item1 == nameOfScene)
        {
            for (int i = 0; i < Props.childCount; i++)
            {
                Transform PositionOfObstacle = Props.GetChild(i);
                string name = obstaclesList.ObstaclesOfScene.Item2[i];
                foreach (ObstacleInventory obstacle in ListOfObstacles)
                {
                    if (obstacle.ObstacleName == name)
                    {
                        OpenPropsMenu installationPoint = PositionOfObstacle.gameObject.GetComponent<OpenPropsMenu>();
                        if (installationPoint.IsUsed())
                        {
                            Destroy(installationPoint.AttachedObstacle());
                        }
                        installationPoint.AttachMountedObstacle(Instantiate(obstacle.ObstaclePrefab, PositionOfObstacle.position + Vector3.forward, obstacle.ObstaclePrefab.transform.rotation, Obstacles), name);
                        installationPoint.Used();
                    }
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
            PlayerPrefs.SetInt("TheBestCounter", (int)TheBestCounter);
            PlayerPrefs.Save();
            Canvas.UpdateTheBestCounter(TheBestCounter);
        }
        {
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
    private void OnDisable()
    {
        obstaclesList.SetScene(nameOfScene);
        if (PlayerPrefs.HasKey($"{nameOfScene}StarsTheBestScore"))
        {
            int a = PlayerPrefs.GetInt($"{nameOfScene}StarsTheBestScore");
            if (a<StarsCounter)
            {
                PlayerPrefs.SetInt($"{nameOfScene}StarsTheBestScore", StarsCounter);
                PlayerPrefs.SetFloat("StarScoreOfUser", PlayerPrefs.GetFloat("StarScoreOfUser")+StarsCounter-a);
            }
        }
        else
        {
            PlayerPrefs.SetInt($"{nameOfScene}StarsTheBestScore", StarsCounter);
            PlayerPrefs.SetFloat("StarScoreOfUser", PlayerPrefs.GetFloat("StarScoreOfUser") + StarsCounter );
        }
        if (PlayerPrefs.HasKey("Score of user"))
        {
            PlayerPrefs.SetFloat("Score of user", PlayerPrefs.GetFloat("Score of user")+CurrentCounter);
        }
        else
        {
            PlayerPrefs.SetFloat("Score of user", 0f);
        }
        PlayerPrefs.Save();
    }
}
