using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPropsMenu : MonoBehaviour
{
    private GameObject menu;
    //private Transform props;
    private GameObject obstacle;
    private string nameObstacle="Notype";

    private bool isUsed = false;
    // Start is called before the first frame update
    public void Start()
    {
        menu = CanvasManager.Instance._ObstaclesMenu;
    }
    public void AttachMountedObstacle(GameObject newObstacles, string name)
    {
        obstacle = newObstacles;
        nameObstacle = name;
    }
    public GameObject AttachedObstacle()
    {
        return obstacle;
    }
    public string TypeAttachedObstacle()
    {
        return nameObstacle;
    }
    public void Used()
    {
        isUsed = true;
    }
    public void UnUsed()
    {
        isUsed = false;
    }
    public bool IsUsed()
    {
        return isUsed;
    }
    // Update is called once per frame
    void OnMouseDown()
    {
        if (menu.activeSelf) return;
        menu.SetActive(true);
        menu.GetComponent<ObstaclePlacement>().SetNewSpawnPlace(transform);//CanvasManager
        gameObject.GetComponent<Renderer>().material.color=new Color(1f, 0.5f, 0.5f, 0.5f);
        
        //for(int i=0; i<props.childCount;i++)
        //{
        //    Transform currentProp = props.GetChild(i);
        //    if (currentProp!=transform)
        //    {
        //        currentProp.gameObject.SetActive(false);
        //    }
        //}
    }
}
