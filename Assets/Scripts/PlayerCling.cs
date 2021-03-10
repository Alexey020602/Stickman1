using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCling : MonoBehaviour
{
    public LayerMask IgnoreLayers;

    private PlayerManager _playerManager;
    public Rigidbody2D CathObjects;
    public Image img;
    public HingeJoint2D HingeJoint;
    public bool catched = false;
    private void Start()
    {
        //_fixedjoint = gameObject.GetComponent<FixedJoint2D>();
        _playerManager = PlayerManager.Instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameobject = collision.gameObject;
        if (gameobject.layer == IgnoreLayers) return;

        Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();
        if (rigidbody == null) return;

        if (!catched || CathObjects == null)
        {
            CathObjects = rigidbody;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject gameobject = collision.gameObject;
        if (gameobject.layer == IgnoreLayers) return;

        Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();
        if (rigidbody == null) return;

        if (CathObjects == rigidbody/* && !catched*/)
        {
            CathObjects = null;
        }
    }

    public void Catch()
    {
        if (CathObjects != null)
        {
            catched = true;
            HingeJoint.connectedBody = CathObjects;
            HingeJoint.enabled = true;
        }
    }

    public void UnCatch()
    {
        catched = false;
        HingeJoint.enabled = false;
        HingeJoint.connectedBody = null;
    }
}
