using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PainObject : MonoBehaviour
{

    private float LastVelocity;
    private float LastDeltaVelocity;
    private Rigidbody2D Body;
    private StickmanBody _stickmanBody;

    private void Start()
    {
        Body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float CurrectVelocity = ModuleVector(Body.velocity);
        float CurrentDeltaVelocity = Mathf.Abs(CurrectVelocity - LastVelocity);
        float DeltaPick = Mathf.Abs(CurrentDeltaVelocity - LastDeltaVelocity);

        if (DeltaPick > 4)
        {
            if (_stickmanBody != null)
            {
                _stickmanBody.Damage(DeltaPick);
                _stickmanBody = null;
            }
        }
        LastDeltaVelocity = CurrentDeltaVelocity;
        LastVelocity = ModuleVector(Body.velocity);

    }


    public float ModuleVector(Vector2 Vector)
    {
        return math.sqrt(Vector.x * Vector.x + Vector.y * Vector.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _stickmanBody = collision.gameObject.GetComponent<StickmanBody>() ?? null;
    }
}
