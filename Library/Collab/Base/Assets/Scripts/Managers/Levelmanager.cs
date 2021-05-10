using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Levelmanager : MonoBehaviour
{
    public Button[] Buttons = new Button[0];
    // Start is called before the first frame update
    void Start()
    {
        
        for( int i = 0; Buttons.Length > i; i++)
        {
            if(Buttons[i] == null)
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

    

    public void LoadScene(string Level)
    {
        SceneManager.LoadScene(Level);
    }
}
