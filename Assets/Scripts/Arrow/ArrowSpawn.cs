using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawn : MonoBehaviour
{
    [Header("Arrow preferences")]
    public StartArrow ArrowPrefab;
    public Transform ArrowAppearence;
    [HideInInspector]
    public StartArrow arrow;
    public float MinForseToPush;
    public float ForseToPush;

    private bool CanChooseDirection = true;
    private Camera _mainCamera;
    private Vector3 _arrowDirectionToPush;
    private GameManager _gamemanager;

    private void Start()
    {
        //Arrow = Instantiate(ArrowPrefab, ArrowAppearence);
        _mainCamera = Camera.main;
        _gamemanager = GameManager.Instance;
    }

    private void Update()
    {
        if (CanChooseDirection && arrow != null)
        {
            TurnArrow();
        }
    }

    void OnMouseDown()
    {
        //Debug.Log("MouseDown");
        if (CanChooseDirection)
        {
            arrow = Instantiate(ArrowPrefab, ArrowAppearence);
        }

    }

    private void OnMouseUp()
    {
        //Debug.Log("MouseUp");
        if (arrow != null)
        {
            CanChooseDirection = false;
            _gamemanager.StartGame((1 - arrow._sprite.color.b) * ForseToPush + MinForseToPush, _arrowDirectionToPush);
            Destroy(arrow.gameObject);
        }
    }
    public void TurnArrow()
    {
        Vector3 _mousePosition = Input.mousePosition;
        _mousePosition.z = 10f;
        Vector3 _direction = _mainCamera.ScreenToWorldPoint(_mousePosition);
        _direction.z = arrow.transform.position.z;

        _arrowDirectionToPush = _direction - arrow.transform.position;
        _arrowDirectionToPush = _arrowDirectionToPush.normalized;
        float Angle = _arrowDirectionToPush.y > 0 ? Mathf.Acos(_arrowDirectionToPush.x) * Mathf.Rad2Deg : (-1f) * Mathf.Acos(_arrowDirectionToPush.x) * Mathf.Rad2Deg;

        arrow.transform.rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
        // Arrow.transform.rotation = Quaternion.Euler(_arrowDirectionToPush);
    }

}
