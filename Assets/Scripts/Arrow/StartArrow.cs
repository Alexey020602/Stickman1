using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArrow : MonoBehaviour
{
    public SpriteRenderer _sprite;
    public Animator animator;
    void Start()
    {
        animator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        _sprite = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }

}
