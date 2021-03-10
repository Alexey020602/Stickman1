using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    public string StateName;

    private PlayerManager PlayerManager;
    // Start is called before the first frame update
    void Start()
    {
        PlayerManager = PlayerManager.Instance;
    }

    public void SetControl()
    {
        PlayerManager.SetControlOfBodyPart(StateName);
    }
    public void DisabledControl()
    {
        PlayerManager.DisabledControlOfBodyPart();
    }
}
