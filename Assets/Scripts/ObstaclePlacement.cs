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
                if (installationPoint.IsUsed())
                {
                    Destroy(installationPoint.AttachedObstacle());
                }
                _obstaclesMenu.SetActive(false);

                installationPoint.AttachMountedObstacle(Instantiate(obstacle.ObstaclePrefab, PositionOfObstacles.position + Vector3.forward, obstacle.ObstaclePrefab.transform.rotation, Obstacles), name);
                installationPoint.Used();

                PositionOfObstacles.gameObject.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
            else if (name == "Notype")
            {
                OpenPropsMenu installationPoint = PositionOfObstacles.gameObject.GetComponent<OpenPropsMenu>();
                if (installationPoint.IsUsed())
                {
                    Destroy(installationPoint.AttachedObstacle());
                }
                _obstaclesMenu.SetActive(false);
                installationPoint.AttachMountedObstacle(null, "Notype");
                installationPoint.UnUsed();
                PositionOfObstacles.gameObject.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }
    }
}
