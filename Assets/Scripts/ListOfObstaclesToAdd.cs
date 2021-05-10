using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfObstaclesToAdd : Singleton<ListOfObstaclesToAdd>
{
    //public string sceneName { get; set; }
    public (string, string[]) ObstaclesOfScene;
    public void AddSceneObstacles( Transform Props)
    {
        //sceneName = scene;
        ObstaclesOfScene.Item2= new string[Props.childCount];
        for(int i=0; i<Props.childCount; i++)
        {
            string typeOfObstacle = Props.GetChild(i).GetComponent<OpenPropsMenu>().TypeAttachedObstacle();
            ObstaclesOfScene.Item2[i]=typeOfObstacle;
        }
    }
    public void SetScene(string scene)
    {
        ObstaclesOfScene.Item1 = scene;
    }
    //public void 
    // Start is called before the first frame update
    void Start()
    {
        //ListOfObstacles = new (string, string[])();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
