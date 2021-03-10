using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEvents : MonoBehaviour
{
    public UnityEvent Pressed;
    public UnityEvent Realized;

    public void pressed()
    {
        Pressed.Invoke();
    }
    public void realized()
    {
        Realized.Invoke();
    }
}
