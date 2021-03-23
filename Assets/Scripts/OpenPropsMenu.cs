using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPropsMenu : MonoBehaviour
{
    private GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        menu = CanvasManager.Instance.transform.Find("ObtanclesMenu").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        menu.SetActive(true);
    }
}
