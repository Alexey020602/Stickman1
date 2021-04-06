using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class ObstaclePlacement : MonoBehaviour
{
    public Transform PositionOfObstacles;
    public Transform Obstacles;
    public Scene SceneToAddObstacles;
    [System.Serializable]
    public class ObstacleInventory
    {
        public string ObstacleName;
        public GameObject ObstaclePrefab;
        //[HideInInspector]
        //public GameObject Obstacle;
    }

    public void SetNewSpawnPlace(Transform _transform)
    {
        PositionOfObstacles = _transform;
    }
    public List<ObstacleInventory> ListOfObstacles = new List<ObstacleInventory>();
    //[HideInInspector]
    private GameObject _obstaclesMenu;
    void Start()
    {
        _obstaclesMenu = CanvasManager.Instance._ObstaclesMenu;
        //SceneToAddObstacles = Singleton<SceneManager>.Instance;
    }

    // Update is called once per frame
    public void FindObstacle(string name)
    {
        foreach (ObstacleInventory obstacle in ListOfObstacles)
        {
            if (obstacle.ObstacleName == name)
            {
                OpenPropsMenu installationPoint = PositionOfObstacles.gameObject.GetComponent<OpenPropsMenu>();
                if(installationPoint.IsUsed())
                {
                    Destroy(installationPoint.AttachedObstacle());
                }
                _obstaclesMenu.SetActive(false);
                GameObject gO = Instantiate<GameObject>(obstacle.ObstaclePrefab, PositionOfObstacles.position + Vector3.forward, obstacle.ObstaclePrefab.transform.rotation, Obstacles);
                gO.GetComponent<SceneName>().sceneName = SceneManager.GetActiveScene().name; 
                installationPoint.AttachMountedObstacle(gO, name);
                SceneManager.MoveGameObjectToScene(Instantiate(gO), SceneToAddObstacles);
                installationPoint.Used();
                PositionOfObstacles.gameObject.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }
    }
}
