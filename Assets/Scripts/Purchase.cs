using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BuyForPoints(/*GameObject obj,*/ float sum )
    {
        if (PlayerPrefs.HasKey("Score of user"))
        {
            if(PlayerPrefs.GetFloat("Score of user")<sum)
            {
                Debug.Log("Недостаточно денег");
            }
            else
            {
                PlayerPrefs.SetFloat("Score of user", PlayerPrefs.GetFloat("Score of user") - sum);
                Debug.Log("Покупка совершена");
            }
        }
        else
        {
            PlayerPrefs.SetFloat("Score of user", 0f);
        }
    }
    public void BuyForStars(float sum)
    {
        if (PlayerPrefs.HasKey("StarScoreOfUser"))
        {
            if (PlayerPrefs.GetFloat("StarScoreOfUser") < sum)
            {
                Debug.Log("Недостаточно звезд");
            }
            else
            {
                PlayerPrefs.SetFloat("StarScoreOfUser", PlayerPrefs.GetFloat("StarScoreOfUser") - sum);
                Debug.Log("Покупка совершена");
            }
        }
        else
        {
            PlayerPrefs.SetFloat("StarScoreOfUser", 0f);
        }
    }
}
