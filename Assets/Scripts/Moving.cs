using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Moving : MonoBehaviour
{
    public KeyCode RightKey = KeyCode.D;
    public KeyCode LeftKey = KeyCode.A;
    public KeyCode LeftRotateKey = KeyCode.Z;
    public KeyCode RighRotateKey = KeyCode.C;
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode RotateKey = KeyCode.W;

    private bool on_Ground = true;
    public bool is_right = true;
    private int numjump = 0;
    private bool is_atacked=false;
    private bool is_regerated = false;

    private Vector2 velocity;

    public Image img;
    public float health=10f;

   
    private Animator anim;

    private float rotation;

    private Transform tr;

    private Rigidbody2D rb;

    public float Max_Speed = 20f;
    public float Max_Rotation = 60f;

    public float ForceRun = 10f;
    public float ForceJump = 15f;
    public float ForceRotation = 40f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        tr = gameObject.transform;
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(RightKey))
        {
            velocity = new Vector2(1 * ForceRun, 0);
        }
        else if (Input.GetKey(LeftKey))
        {
            velocity = new Vector2(-1 * ForceRun, 0);
        }
        else
        {
            velocity = new Vector2(0, 0);
        }

        if (Input.GetKey(RighRotateKey))
        {
            rotation = ForceRotation;
        }
        else if (Input.GetKey(LeftRotateKey))
        {
            rotation = -ForceRotation;
        }
        else
        {
            rotation = 0;
        }


        if (rb.velocity.x == 0)
        {
            anim.SetBool("is_walking", false);
        }
        else
        {
            if (rb.velocity.x > 0)
            {
                if (!is_right)
                {
                    tr.Rotate(new Vector3(0, 180, 0), Space.Self);
                    is_right = true;
                }
            }
            else
            {
                if (is_right)
                {
                    tr.Rotate(new Vector3(0, 180, 0), Space.Self);
                    is_right = false;
                }
            }
            anim.SetBool("is_walking", true);
        }
        if(is_atacked)
        {
            img.fillAmount -= 1 / (Time.deltaTime * health);
        }
        if (is_regerated)
        {
            img.fillAmount += 1 / (Time.deltaTime * health);
        }
    }
    void FixedUpdate()
    {
        Forces();
        if (Input.GetKeyDown(JumpKey) && (on_Ground || numjump<2))
        {
            if (numjump == 1)
                print("Yes");
            numjump++;
            rb.AddForce(new Vector2(0, ForceJump), ForceMode2D.Impulse);
                anim.SetTrigger("jump");
        }
    }

    void FrictionForces()
    {
        rb.AddTorque(-rb.rotation * 0.1f);
        rb.AddForce(-rb.velocity * 0.1f);
    }
    void Forces()
    {
        if (Mathf.Abs(rb.velocity.x) < Max_Speed)
        {
            rb.AddForce(velocity);
        }
        if (rb.rotation < Max_Rotation)
        {
            rb.AddTorque(rotation);
        }
        FrictionForces();
        if (Mathf.Abs(rb.velocity.magnitude) < Max_Speed)
        {
            rb.AddForce(velocity);
        }
        if (rb.rotation < Max_Rotation)
        {
            rb.AddTorque(rotation);
        }
        FrictionForces();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag=="Regenerate")
        {
            is_regerated = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Regenerate")
        {
            is_regerated = false;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") is_atacked = false;
        on_Ground = false;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") is_atacked = true;
        on_Ground = true;
        numjump = 0;
    }

}