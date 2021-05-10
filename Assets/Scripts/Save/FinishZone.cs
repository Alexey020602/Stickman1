using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishZone : MonoBehaviour
{

    [HideInInspector]
    public CanvasManager canvasManager;
    void Start()
    {
        canvasManager = CanvasManager.Instance;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        StickmanBody stickmanBody = collision.gameObject.GetComponent<StickmanBody>();

        if (stickmanBody == null) return;

        canvasManager.RestartMenu(true);
    }

}
