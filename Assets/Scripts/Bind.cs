using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bind : MonoBehaviour
{
    private Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
