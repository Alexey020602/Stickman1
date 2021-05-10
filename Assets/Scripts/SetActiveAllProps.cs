using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveAllProps : MonoBehaviour
{
    public void SetActiveProps()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
    }
    private void OnDestroy()
    {
        string nameOfScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (nameOfScene == null)
        {
            Debug.LogError("Scene has not name");
            return;
        }
        if (nameOfScene == "MainMenu")
        {
            return;
        }
        if (transform == null)
        {
            return;
        }
        ListOfObstaclesToAdd obstaclesList=ListOfObstaclesToAdd.Instance;
        obstaclesList.AddSceneObstacles( transform);
    }
}
