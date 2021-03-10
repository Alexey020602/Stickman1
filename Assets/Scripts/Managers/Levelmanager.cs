using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Levelmanager : MonoBehaviour
{
    public StartArrow ArrowPrefab;
    public Transform ArrowAppearence;
    public StartArrow Arrow;
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
        if (CanChooseDirection && Arrow != null)
        {
            TurnArrow();
        }

        //проверка
        if (Input.GetKeyDown(KeyCode.A))
        {
            CanChooseDirection = false;
            _gamemanager.StartGame((1- Arrow._sprite.color.b) * ForseToPush + MinForseToPush, _arrowDirectionToPush);
            Destroy(Arrow.gameObject);
        }
        //можно удалить
    }

    void OnMouseDown()
    {
        if (CanChooseDirection)
        {
            Debug.LogWarning("Click");
            Arrow = Instantiate(ArrowPrefab, ArrowAppearence);
          
        }

    }

    private void OnMouseUp()
    {
        if (Arrow !=null)
        {
             CanChooseDirection = false;
            _gamemanager.StartGame((1 - Arrow._sprite.color.b) * ForseToPush + MinForseToPush, _arrowDirectionToPush);
            Destroy(Arrow.gameObject);
        }

    }
    public void TurnArrow()
    {

        Vector3 _mousePosition = Input.mousePosition;
        _mousePosition.z = 10f;
        Vector3 _direction = _mainCamera.ScreenToWorldPoint(_mousePosition);
        _direction.z = Arrow.transform.position.z;

        _arrowDirectionToPush = _direction - Arrow.transform.position;
        _arrowDirectionToPush = _arrowDirectionToPush.normalized;
        float Angle = _arrowDirectionToPush.y > 0 ? Mathf.Acos(_arrowDirectionToPush.x) * Mathf.Rad2Deg : (-1f) * Mathf.Acos(_arrowDirectionToPush.x) * Mathf.Rad2Deg;

         Arrow.transform.rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
       // Arrow.transform.rotation = Quaternion.Euler(_arrowDirectionToPush);
    }



    //public Button[] Buttons = new Button[0];

    //void Start()
    //{

    //    for( int i = 0; Buttons.Length > i; i++)
    //    {
    //        if(Buttons[i] == null)
    //        {
    //            continue;
    //        }
    //        bool a = Progress.Scenes[i];
    //        if (a)
    //        {
    //            Buttons[i].interactable = true;
    //        }
    //        else
    //        {
    //            Buttons[i].interactable = false;
    //        }
    //    }
    //}
    public void LoadScene(string Level)
    {
        SceneManager.LoadScene(Level);
    }
}
