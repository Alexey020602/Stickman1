using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerControl : MonoBehaviour
{
    public KeyCode RightKey = KeyCode.D;
    public KeyCode LeftKey = KeyCode.A;
    public KeyCode JumpKey = KeyCode.Space;

    public float MaxSpeed = 10f;

    public float ForceRun = 2f;
    public float ForceJump = 5f;

    private Rigidbody2D rigidbody2D;

    private Vector2 direction;

    private bool isGround = false;

    private void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateKey();
    }


    private void UpdateKey()
    {
        if (Input.GetKey(RightKey))
        {
            direction = new Vector2(1 * ForceRun, 0);
        }
        else if (Input.GetKey(LeftKey))
        {
            direction = new Vector2(-1 * ForceRun, 0);
        }
        else
        {
            direction = new Vector2(0, 0);
        }

        
    }

    private void FixedUpdate()
    {
        Force();
        if (Input.GetKey(JumpKey) && isGround)
        {
            rigidbody2D.AddForce(new Vector2(0, 1 * ForceJump), ForceMode2D.Impulse);
        }
    }

    private void Force()
    {
        if (rigidbody2D.velocity.x > 0.001)
        {
            rigidbody2D.AddForce(new Vector2(-0.2f, 0));
        }
        else if (rigidbody2D.velocity.x < -0.001)
        {
            rigidbody2D.AddForce(new Vector2(0.2f, 0));
        }



        if (Mathf.Abs(rigidbody2D.velocity.x) < MaxSpeed)
        {
            rigidbody2D.AddForce(direction);
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        isGround = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        isGround = true;
    }

}
