using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public Transform tr;
    private Transform trans;
    public float lag = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        trans = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        trans.position = Vector3.Lerp(new Vector3(tr.position.x, tr.position.y, trans.position.z), trans.position, lag * Time.deltaTime);
    }
}
