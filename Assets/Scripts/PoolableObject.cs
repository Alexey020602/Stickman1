using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public Vector2 OffSetImpulse;

    private void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(OffSetImpulse, ForceMode2D.Impulse);
    }
}