using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

public class StickmanBody : MonoBehaviour
{
    public Vector2 OffSetImpulse;


    [Header("Pain")]
    public float TimeToSlowPain = 0.2f;
    public float SpeedSlowPainBone = 4f;
    public float SpeedSlowPain = 4f;

    public float TimeBetweenDamage = 0.1f;

    private float LastVelocity;
    private float LastDeltaVelocity;
    private Rigidbody2D Body;
    private SpriteRenderer Bone;
    private SpriteRenderer Pain;
    private float RedColor;

    private GameManager _gameManager;
    private Levelmanager _levelManager;
    private PlayerManager _playerManager;

    private bool CanSlowPain = true;
    private bool CanSlowPainBone = true;
    private bool CanTakeDamage = true;

    //Nina starts
    public GameObject[] LowerPartsOfBody;
    private bool AreAddedToDestroyList = false;
    public Transform _transform;
    //end

    void Start()
    {
        _gameManager = GameManager.Instance;
        _levelManager = Levelmanager.Instance;
        _playerManager = PlayerManager.Instance;
        _transform = GetComponent<Transform>();
        Body = GetComponent<Rigidbody2D>();
        Body.AddForce(OffSetImpulse, ForceMode2D.Impulse);

        Bone = gameObject.transform.Find("Bone").GetComponent<SpriteRenderer>() ?? null;
        Pain = GetComponent<SpriteRenderer>();
        RedColor = GetComponent<SpriteRenderer>().color.r;
        
    }


    void FixedUpdate()
    {
        if (Pain.color.r > 14f / 255f && CanSlowPain)
        {

            Color color = Pain.color;

            color.r -= SpeedSlowPain / 255f;
            if (color.r < RedColor)
            {
                color.r = RedColor;
            }
            Pain.color = color;
        }
        if (Bone != null)
        {
            if (Bone.color.a > 0f && CanSlowPainBone)
            {
                Color color = Bone.color;
                color.a -= SpeedSlowPainBone / 255f;
                if (color.a < 0f)
                {
                    color.a = 0f;
                }
                Bone.color = color;
            }
        }
        

        float CurrectVelocity = ModuleVector(Body.velocity);
        float CurrentDeltaVelocity = Mathf.Abs(CurrectVelocity - LastVelocity);
        float DeltaPick = Mathf.Abs(CurrentDeltaVelocity - LastDeltaVelocity);

        if (DeltaPick > 4)
        {
            Damage(DeltaPick);
        }
        LastDeltaVelocity = CurrentDeltaVelocity;
        LastVelocity = ModuleVector(Body.velocity);

    }

    //Nina starts


    void OnJointBreak2D(Joint2D brokenJoint)
    {
        for (int i = 0; i < LowerPartsOfBody.Length; i++)
        {
            StickmanBody stickmanBody = LowerPartsOfBody[i].gameObject.GetComponent<StickmanBody>();


            _levelManager.DestroyedPartsOfBodyList.Add(stickmanBody);
            stickmanBody.AreAddedToDestroyList = true;
            stickmanBody.DestroyedLowerPartsOfBody();

        }

        _levelManager.DestroyStickmanBody();

    }

    public void DestroyedLowerPartsOfBody()
    {
        if (AreAddedToDestroyList)
        {
            for (int i = 0; i < LowerPartsOfBody.Length; i++)
            {
                _levelManager.DestroyedPartsOfBodyList.Add(LowerPartsOfBody[i].GetComponent<StickmanBody>());

                StickmanBody stickmanBody = LowerPartsOfBody[i].gameObject.GetComponent<StickmanBody>();
                stickmanBody.AreAddedToDestroyList = true;
                stickmanBody.DestroyedLowerPartsOfBody();
    
            }
        }
    }
    //end



    public void Damage(float Damage)
    {
        if (CanTakeDamage)
        {
            _gameManager.UpdateCurrentCounter(Damage * 10);

            if (Damage > 2)
            {
                Color color = Pain.color;
                color.r = 70f / 255f;
                Pain.color = color;
                CanSlowPain = false;
                StartCoroutine(ToCanSlowPain());
            }

            if(Damage > 10)
            {
                Color color = Bone.color;
                color.a = 255f/255f;
                Bone.color = color;
                CanSlowPainBone = false;
                StartCoroutine(ToCanSlowPainBone());
            }
            StopGetDamage(TimeBetweenDamage);
        }
    }

    public void StopGetDamage(float seconds)
    {
        CanTakeDamage = false;
        StartCoroutine(ToTakeDamage(seconds));
    }

    IEnumerator ToTakeDamage(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        CanTakeDamage = true;
    }

    IEnumerator ToCanSlowPain()
    {
        yield return new WaitForSeconds(TimeToSlowPain);
        CanSlowPain = true;
    }

    IEnumerator ToCanSlowPainBone()
    {
        yield return new WaitForSeconds(TimeToSlowPain);
        CanSlowPainBone = true;
    }

    public float ModuleVector(Vector2 Vector)
    {
        return math.sqrt(Vector.x * Vector.x + Vector.y * Vector.y);
    }

}
