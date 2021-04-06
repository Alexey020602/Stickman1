using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveAllProps : MonoBehaviour
{
   public void SetActiveProps(bool state)
    {
        for(int i=0; i< transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(state);
        }
    }
}
