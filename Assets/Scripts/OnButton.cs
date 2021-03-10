using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnButton : MonoBehaviour
{
    public UnityEvent OnPressed;
    public UnityEvent OnReleased;

    public float changeSpeed = 5f;

    public string ButtonPressed = "Selected";
    public string ButtonOff = "Disabled";

    //private Image _image;


    private Animator _animator;
    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>() ?? null;
        //_image = gameObject.GetComponent<Image>();
    }

    private void OnMouseDown()
    {
        if(_animator) _animator.SetTrigger("Selected");
        //if(_image)_image.color = Color.Lerp(Color.white, Color.green, changeSpeed * Time.deltaTime);
        OnPressed.Invoke();
    }
    private void OnMouseUp()
    {
        if (_animator) _animator.SetTrigger("Disabled");
        OnReleased.Invoke();
    }
}
