using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public int SmokeForce = 200;
    public Vector2 DirectionToForce = new Vector2(0, 0);
    //public Vector2 SmokeForce = new Vector2(0, 500);

    List<Rigidbody2D> Bodyes = new List<Rigidbody2D>();

    private void Start()
    {
        float rot = gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        DirectionToForce.x = -(float)Math.Sin(rot);
        DirectionToForce.y = (float)Math.Cos(rot);
        DirectionToForce.x *= SmokeForce;
        DirectionToForce.y *= SmokeForce;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>() ?? null;
        if(rigidbody != null)
        {
            Bodyes.Add(rigidbody);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>() ?? null;
        if (rigidbody != null)
        {
            Bodyes.Remove(rigidbody);
        }
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody2D body in Bodyes)
        {
            body.AddForce(DirectionToForce);
        }
    }
}
