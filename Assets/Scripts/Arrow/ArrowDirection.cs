using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDirection : MonoBehaviour
{
    [HideInInspector]
    public Camera _mainCamera;
    [HideInInspector]
    public PlayerManager _player;
    public Transform _arrowTransform;

    [HideInInspector]
    public Vector3 _CorrectlocalPosition;
    [HideInInspector]
    public bool CanChooseDirection = true;
    [HideInInspector]
    public Vector3 _localPosition;

    private float PushForce = -0.001f;
    private bool GrowingPower;
    private SpriteRenderer _sprite;

    private void Start()
    {
        _mainCamera = Camera.main;
        try { _player = GameObject.Find("Stickman").GetComponent<PlayerManager>(); } catch { Debug.Log("No Player at the scene"); }
        _sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

    }

 
    void Update()
    {
        if ( CanChooseDirection)
        {
            TurnArrow();
            GetForce();
        }
    }

    public void GetForce()
    {
        if (PushForce < 0)
        {
            PushForce = 0;
            GrowingPower = true;
        }

        if (PushForce > 200f)
        {
            PushForce = 200f;
            GrowingPower = false;
        }

        if (GrowingPower == true)
        {
            PushForce += _sprite.color.b ;
        }
        else if (GrowingPower == false)
        {
            PushForce -= _sprite.color.b;
        }

        Debug.LogError(PushForce);
        //Debug.LogError(_sprite.color.b);
    }

    public void TurnArrow()
    {

        Vector3 _mousePosition = Input.mousePosition;
        _mousePosition.z = 10f;
        Vector3 _direction = _mainCamera.ScreenToWorldPoint(_mousePosition);
        _direction.z = transform.position.z;

        _localPosition = _direction - transform.position;
        _localPosition = _localPosition.normalized;
        float Angle = _localPosition.y > 0 ? Mathf.Acos(_localPosition.x) * Mathf.Rad2Deg : (-1f) * Mathf.Acos(_localPosition.x) * Mathf.Rad2Deg;

        if (_localPosition.x > 0)
        {
            transform.localScale = transform.localScale.x < 0 ? transform.localScale : new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = transform.localScale.x < 0 ? transform.localScale : new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
        }

        transform.rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
    }
 

}
