using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstaclePlacement : MonoBehaviour
{
    public Transform PositionOfObstacles;
    public Transform Obstacles;
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
                installationPoint.AttachMountedObstacle(Instantiate<GameObject>(obstacle.ObstaclePrefab, PositionOfObstacles.position+Vector3.forward, obstacle.ObstaclePrefab.transform.rotation, Obstacles), name);
                installationPoint.Used();
                Transform props = PositionOfObstacles.parent;
                for (int i = 0; i < props.childCount; i++)
                {
                    Transform currentProp = props.GetChild(i);
                    if (currentProp != PositionOfObstacles)
                    {
                        currentProp.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
