using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAppearance : MonoBehaviour
{
    private ArrowDirection _arrowDirection;

    private void Start()
    {
       // _arrowDirection =ArrowManager.instance.ArrowDirectionScript;
        //через транформер
        try { _arrowDirection = GameObject.Find("ArrowPosition").GetComponent<ArrowDirection>(); } catch { Debug.Log("No  ArrowDirection at the scene"); }
    }
    void OnMouseDown()
    {
        if (_arrowDirection.CanChooseDirection)
        {
            _arrowDirection._arrowTransform.gameObject.SetActive(true);
            //через транформер
            //

            GameObject.Find("ArrowPosition").transform.GetChild(0).gameObject.GetComponent<Animation>().Play("Arrow");
        }
    }

    private void OnMouseUp()
    {
        _arrowDirection._CorrectlocalPosition = _arrowDirection._localPosition;
        _arrowDirection.CanChooseDirection = false;
        Debug.LogWarning(_arrowDirection._CorrectlocalPosition);
    }
}
