using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SimpleSingleton<GameManager>
{


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
    }

    public List<ObstacleInventory> ListOfObstacles = new List<ObstacleInventory>();

    void Start()
    {
        obstaclesList = ListOfObstaclesToAdd.Instance;

        SetObstacles();
    }

    public void SetObstacles()
    {
        return;
        if (obstaclesList == null)
        {
            return;
        }
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
        PlayerPrefs.Save();
    }
}
