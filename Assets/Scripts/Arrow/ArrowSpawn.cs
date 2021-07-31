using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArrowSpawn : MonoBehaviour
{
    [System.Serializable]
    public class AngleBordersCL
    {
        public float LowerAngle = 180f;
        public float UpperAngle = 180f;
    }
    [Header("Arrow preferences")]
    public StartArrow ArrowPrefab;
    public Transform ArrowAppearence;
    public float MinForseToPush;
    public float ForseToPush;
    public bool InfinityAngles = true;
    public AngleBordersCL AngleBorders;
    [ReadOnly]
    public bool isChoosing = false;

    private StartArrow arrow;
    private bool canChooseDirection = true;
    private Camera _mainCamera;
    private Vector3 _arrowDirectionToPush;
    private GameManager _gamemanager;
    public float angle { get; protected set; }

    public virtual void Start()
    {
        _mainCamera = Camera.main;
        _gamemanager = GameManager.Instance;
    }
    public void Update()
    {
        if (canChooseDirection && arrow != null)
            TurnArrow();
    }

    void OnMouseDown()
    {
        if (canChooseDirection)
        {
            isChoosing = true;
            arrow = Instantiate(ArrowPrefab, ArrowAppearence);
        }
    }
    void OnMouseUp()
    {
        if (arrow != null)
        {
            isChoosing = false;
            canChooseDirection = false;
            PlayerManager.Instance.StartGame((1 - arrow._sprite.color.b) * ForseToPush + MinForseToPush, -_arrowDirectionToPush);
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
        angle = _arrowDirectionToPush.y > 0 ? Mathf.Acos(_arrowDirectionToPush.x) * Mathf.Rad2Deg : (-1f) * Mathf.Acos(_arrowDirectionToPush.x) * Mathf.Rad2Deg;

        if (!InfinityAngles)
        {
            if(angle < AngleBorders.LowerAngle)
                angle = AngleBorders.LowerAngle;
            else if(angle > AngleBorders.UpperAngle)
                angle = AngleBorders.UpperAngle;
        }
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void SetCanChooseDirection(bool state) => canChooseDirection = state;

}
