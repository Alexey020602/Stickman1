using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    private AudioSource audio_source;
    void Start()
    {
        audio_source = GetComponent<AudioSource>();
    }

    void OnJointBreak2D(Joint2D brokenJoint)
    {
        audio_source.Play();
    }
}
