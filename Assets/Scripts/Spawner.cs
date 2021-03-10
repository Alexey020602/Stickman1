using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public PoolableObject PoolObject;
    public Vector2 Impulse = new Vector2(1, 0);
    

    public void Spawn()
    {
        PoolObject.OffSetImpulse = Impulse;
        Instantiate(PoolObject, gameObject.transform.position, gameObject.transform.rotation);
    }
}
