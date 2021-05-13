using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CollectingStars : MonoBehaviour
{
    public GameObject Stickman;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }
    int ChildIndex()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i) == transform) return i;
        }
        return -1;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Catched");
        if (collision.transform.parent == Stickman.transform)
        {
            Destroy(gameObject);
            gameManager.StarsCounter++;
            Debug.Log("Добавлена звезда");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent == Stickman.transform)
        {
            Destroy(gameObject);
            gameManager.StarsCounter++;
            Debug.Log("Добавлена звезда");
            //PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name} {ChildIndex()} star collected", 1);
            //if(PlayerPrefs.HasKey("Number of stars"))
            //{
            //    PlayerPrefs.SetFloat("Number of stars", PlayerPrefs.GetFloat("Number of stars")+1f);
            //}
            //else
            //{
            //    PlayerPrefs.SetFloat("Number of stars", 0f);
            //}
        }
    }
}
