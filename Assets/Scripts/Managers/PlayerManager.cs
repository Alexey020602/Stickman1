using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class BodyParts
{
    public string Name;
    public bool active;
    public Rigidbody2D Rigidbody;
    public StickmanBody StickmanBody;
    public StickmanAngle StickmanAngle;
    public HingeJoint2D Joint;
    public float angle;
    public float force;
    public float speed;
    [HideInInspector]
    public PhysicsMaterial2D physicsMaterial;
}

#region PlayerState Classes

#region Game PlayerStates
[System.Serializable]
public class ClassPlayerState
{
    public string StateName;
    public List<ClassPartState> Part = new List<ClassPartState>();
}

[System.Serializable]
public class ClassPartState
{
    public string Name;
    public GameObject Part;
    public float angle;
    public float force;
    public float speed;
}
#endregion

#region Static PlayerStates

[System.Serializable]
public class BodyPartStaticStateCL
{
    public GameObject Part;
    public Vector3 Position;
    public Quaternion Rotation;
}
[System.Serializable]
public class PlayerStaticStateCL
{
    public string PoseName;
    public List<BodyPartStaticStateCL> BodyParts;
}


#endregion

#endregion




[System.Serializable]
public class StaminaCL
{
    [Min(0)]
    public float Size;
    [Space(4f)]
    [Min(0)]
    public float HoverTime;
    [Min(0)]
    public float RecoveryTime;
    [Space(4f)]
    [Tooltip("the minimum value with which we can grapple after losing stamina ")]
    [Min(0)]
    public float LowerSizeToCatch;

    private float fullSize;
    private bool canCatch;
    private bool isInfinityStamina;

    public void SetFullSize() => fullSize = Size;
    public bool HasStamina() => canCatch;
    public void DownGrade(float factor = 1f)
    {
        if (isInfinityStamina)
            return;
        Size -= HoverTime * Time.deltaTime * factor;
        if (Size < 0)
        {
            Size = 0;
            canCatch = false;
        }
    }
    public void UpGrade(float factor = 1f)
    {
        if (isInfinityStamina)
            return;
        Size += HoverTime * Time.deltaTime * factor;

        if (Size > fullSize)
            Size = fullSize;

        if (!canCatch && Size > LowerSizeToCatch)
            canCatch = true;
    }
    public float GetFillAmount() => Size / fullSize;

    public void IsInfinityStamina(bool state)
    {
        isInfinityStamina = state;
    }
}

public class PlayerManager : SimpleSingleton<PlayerManager>
{
    public enum StartGameTypes
    {
        ArrowPush,
        ButtonWithPush,
        ButtonWithoutPush,
        Instantly
    }
    public enum ControlTypes
    {
        Keyboard,
        Buttons
    }

    // inspector
    [Header("Body part")]
    public float DefaultSpeed = 100;
    public float DefaultForce = 5;
    public bool UseDefaultParameters = true;

    public float MinForce = 20;

    //public string[] BodyPartsString = { "Head", "Body0", "Body1", "Body2", "LegForward1", "LegForward0", "LegBehind0", "LegBehind1", "HandBehind0", "HandBehind1", "HandForward0", "HandForward1" };

    // хранит в себе данные о всех частях тела
    public List<BodyParts> Body = new List<BodyParts>();

    [Header("Hands")]
    public PlayerCling LeftHand;
    public PlayerCling RightHand;

    [Header("Player contol states")]
    public List<ClassPlayerState> PlayerState = new List<ClassPlayerState>();
    #region Player State Inspector Control

#if UNITY_EDITOR
    [Button("SetDynamicStateByName")] public bool bt3_;
    public bool SetAlsoAsStaticPose = true;
    public string DynamicPoseName;

    public void SetDynamicStateByName()
    {
        PlayerState.Add(new ClassPlayerState());
        int index = PlayerState.Count - 1;
        PlayerState[index].StateName = DynamicPoseName;
        PlayerState[index].Part = new List<ClassPartState>();
        for (int i = 0; i < Body.Count; i++)
        {
            float angle = Body[i].Joint ? Body[i].Joint.jointAngle : 0;

            if (Body[i].Joint)
            {
                if (angle > Body[i].Joint.limits.max)
                    angle -= 360f;
                else if (angle < Body[i].Joint.limits.min)
                    angle += 360f;
            }
            PlayerState[index].Part.Add(new ClassPartState()
            {
                speed = DefaultSpeed,
                angle = Body[i].Joint ? angle : 0,
                force = DefaultForce,
                Name = Body[i].Name,
                Part = Body[i].Rigidbody.gameObject
            });
        }
        EditorUtility.SetDirty(this);
        if (SetAlsoAsStaticPose)
        {
            PoseName = DynamicPoseName;
            SetStaticStateByName();
        }
    }
#endif

    #endregion

    [Header("Player static states")]
    public List<PlayerStaticStateCL> PlayerStaticStates = new List<PlayerStaticStateCL>();
    #region Player Static State Inspector Control
#if UNITY_EDITOR
    [Button("SetStaticStateByName")] public bool bt1_;
    [Button("GetStaticStateByName")] public bool bt2_;
    public string PoseName;

    public void SetStaticStateByName()
    {
        PlayerStaticStates.Add(new PlayerStaticStateCL());
        int index = PlayerStaticStates.Count - 1;
        PlayerStaticStates[index].PoseName = PoseName;
        PlayerStaticStates[index].BodyParts = new List<BodyPartStaticStateCL>();

        for (int i = 0; i < Body.Count; i++)
        {
            PlayerStaticStates[index].BodyParts.Add(new BodyPartStaticStateCL()
            {
                Part = Body[i].Rigidbody.gameObject,
                Position = Body[i].Rigidbody.transform.localPosition,
                Rotation = Body[i].Rigidbody.transform.localRotation
            }) ;
        }
        EditorUtility.SetDirty(this);
    }

    public void GetStaticStateByName()
    {
        for(int i = 0; i < PlayerStaticStates.Count; i++)
        {
            if (PoseName == PlayerStaticStates[i].PoseName)
            {
                for (int j = 0; j < Body.Count; j++)
                {

                    Undo.RecordObject(Body[j].Rigidbody.transform, "State changed");
                    Body[j].Rigidbody.transform.localPosition = PlayerStaticStates[i].BodyParts[j].Position;
                    Body[j].Rigidbody.transform.localRotation = PlayerStaticStates[i].BodyParts[j].Rotation;
                }
                return;
            }
        }
        EditorUtility.SetDirty(this);
    }
#endif
    #endregion

    [Header("Game States")]
    public bool InfinityGame = false;
    public bool CanMove = false;
    public bool isMenu = false;
    public StartGameTypes StartGameType = StartGameTypes.ArrowPush;
    public CountDownTimer CountDownTimer;
    public ControlTypes ControlType;
    public string CurrentState;
    public string DefaultState = "Right";

    [Header("Cling parameters")]
    public StaminaCL Stamina;
    public bool CatchOnStart = false;

    [Header("Jumper")]
    public PhysicsMaterial2D JumperMaterial;

    [Header("Debug")]
    public bool DisablePoseCreator = true;


    private Rigidbody2D Head; // обьект для определения малой скорости, чтобы фиксировать конец игры
    private bool LowVelocityActive = true; // помогает нормально работать таймеру конца игры
    private bool _iscatch = false;
    private Coroutine SetToNormalStateCor;
    private bool cath = false;
    private bool isPose = true;
    private Vector3 defaultPosition;
    [HideInInspector]
    public Rigidbody2D catchObjectOnStart;



    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //при старте записываем обьект head, чтобы мы его могли использовать в дальнейшем для проверки конца игры
        Head = Body[1].Rigidbody;

        Stamina.SetFullSize();

        if (isMenu)
            StartGameType = StartGameTypes.Instantly;

        switch (StartGameType)
        {
            case StartGameTypes.ArrowPush:
                Levelmanager.Instance.Pause(true);
                Levelmanager.Instance.StartButton_SetActive(false);
                Levelmanager.Instance.SimpleStartButton.SetActive(false);
                break;
            case StartGameTypes.ButtonWithPush:
                Levelmanager.Instance.Pause(true);
                GetComponent<ArrowSpawn>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                Levelmanager.Instance.StartButton_SetActive(true);
                Levelmanager.Instance.SimpleStartButton.SetActive(false);
                break;
            case StartGameTypes.ButtonWithoutPush:
                Levelmanager.Instance.Pause(true);
                GetComponent<ArrowSpawn>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                Levelmanager.Instance.StartButton_SetActive(false);
                Levelmanager.Instance.SimpleStartButton.SetActive(true);
                break;
            case StartGameTypes.Instantly:
                GetComponent<ArrowSpawn>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                StartGame(0, Vector2.zero);
                break;
        }

        defaultPosition = transform.position;

        foreach (BodyParts part in Body)
        {
            part.physicsMaterial = part.Rigidbody.sharedMaterial;
        }
    }

    private void Update()
    {
        if (ControlType == ControlTypes.Keyboard && CanMove)
        {
            cath = Input.GetKey(KeyCode.Space);

            float hor = Input.GetAxisRaw("Horizontal");
            float vert = Input.GetAxisRaw("Vertical");
                if (vert > 0f)
                {
                    if (CurrentState != "Up")
                    {
                        SetControlOfBodyPart("Up");
                    }
                }
                else if (hor < 0f)
                {
                    if (CurrentState != "Left")
                    {
                        SetControlOfBodyPart("Left");
                    }
                }
                else if (vert < 0f)
                {
                    if (CurrentState != "Down")
                    {
                        SetControlOfBodyPart("Down");
                    }
                }
                else if (hor > 0f)
                {
                    if (CurrentState != "Right")
                    {
                        SetControlOfBodyPart("Right");
                    }
                }
                else
                {
                    SetControlOfBodyPart("");
                }
        }

        if (cath)
        {
            if (Stamina.HasStamina())
                RawCatch();
            else
                RawUnCatch();
        }
        else
            RawUnCatch();

        PoseControl();


        if (!isMenu)
            Levelmanager.Instance.UpdateStamina(Stamina.GetFillAmount());
    }

    private void FixedUpdate()
    {
        if (!CanMove) return;

        CheckLowVelocity();

        if (_iscatch)
        {
            LeftHand.Catch();
            RightHand.Catch();
        }
        else
        {
            LeftHand.UnCatch();
            RightHand.UnCatch();
        }
        if (LeftHand.catched || RightHand.catched)
        {
            if (LeftHand.catched && RightHand.catched)
            {
                Stamina.DownGrade(2);
            }
            else
            {
                Stamina.DownGrade();
            }
        }
        else
            Stamina.UpGrade();
    }

    public void SetInfinityStamina(bool state = true)
    {
        Stamina.IsInfinityStamina(state);
    }

    public void SetStickmanControl(bool state)
    {
        CanMove = state;
        //UnCatch();
    }

    public void SetDefaultPosition()
    {
        transform.position = defaultPosition;
    }

    // устанавливаем состояние персонажа
    public void SetControlOfBodyPart(string statename)
    {
        CurrentState = statename;
    }

    private void PoseControl()
    {
        if (CurrentState != "")
        {
            isPose = true;
            for (int i = 0; i < PlayerState.Count; i++)
            {
                if (PlayerState[i].StateName == CurrentState)
                {
                    for (int k = 0; k < PlayerState[i].Part.Count; k++)
                    {
                        if (!Body[k].active) continue;

                        if (UseDefaultParameters)
                        {
                            Body[k].StickmanAngle.ConAngle = PlayerState[i].Part[k].angle;
                            Body[k].StickmanAngle.ConSpeed = DefaultSpeed;
                            Body[k].StickmanAngle.ConForce = DefaultForce;
                            Body[k].StickmanAngle.IsControl = true;
                        }
                        else
                        {
                            Body[k].StickmanAngle.ConAngle = PlayerState[i].Part[k].angle;
                            Body[k].StickmanAngle.ConSpeed = PlayerState[i].Part[k].speed;
                            Body[k].StickmanAngle.ConForce = PlayerState[i].Part[k].force;
                            Body[k].StickmanAngle.IsControl = true;
                        }
                    }
                    break;
                }
            }
        }
        else if (isPose)
        {
            DisabledControlOfBodyPart();
            isPose = false;
        }
    }

    public void DisabledControlOfBodyPart()
    {
        //CurrentState = null;
        for (int k = 0; k < PlayerState[1].Part.Count; k++)
        {
            if (!Body[k].active) continue;

            Body[k].StickmanAngle.ConAngle = 0;
            Body[k].StickmanAngle.ConSpeed = DefaultSpeed;
            Body[k].StickmanAngle.ConForce = DefaultForce;
            Body[k].StickmanAngle.IsControl = false;

        }
    }
    // при запуске игры кидаем персонажа
    public void StartGame(float Force, Vector3 direction)
    {

        if (!isMenu)
            Levelmanager.Instance.StartGame(false);

        for (int j = 0; j < Body.Count; j++)
        {
            Body[j].Rigidbody.isKinematic = false;
            Body[j].StickmanBody.StopGetDamage(0.1f);
            Body[j].Rigidbody.AddForce(direction * Force, ForceMode2D.Impulse);
        }


        CanMove = true;

        if (CatchOnStart)
        {
            Catch();
            if (catchObjectOnStart)
            {
                LeftHand.Catch(catchObjectOnStart);
                RightHand.Catch(catchObjectOnStart);
            }
        }
    }

    // проверяем скорость персонажа, чтобы закончить игру
    public void CheckLowVelocity()
    {
        if (InfinityGame) return;

        if (Head.velocity.magnitude < 1f)
        {
            if (LowVelocityActive)
            {
                LowVelocityActive = false;
                CountDownTimer.ActiveCounter = true;
                CountDownTimer.timeLeft = 6f;

            }
        }
        else
        {
            LowVelocityActive = true;
            CountDownTimer.ActiveCounter = false;
        }
    }

    #region debug
    // функция для заполнения списков частями тел
    /*public void FindBodyesParts()
    {
        // обнуляем список перед его перезаполнением
        Body = new List<BodyParts>();
        // цикл для заполненияю от 0 до колличество наименований частей тела
        for (int i = 0; i < BodyPartsString.Length; i++)
        {
            // добавляем часть тела
            Body.Add(new BodyParts()
            {
                // ищем часть тела с данным названием и берем у нее нужные компонент
                Name = BodyPartsString[i],
                Rigidbody = gameObject.transform.Find(BodyPartsString[i]).GetComponent<Rigidbody2D>(),
                StickmanBody = gameObject.transform.Find(BodyPartsString[i]).GetComponent<StickmanBody>(),
                StickmanAngle = gameObject.transform.Find(BodyPartsString[i]).GetComponent<StickmanAngle>(),
                active = true
            });
            // проверяем у новосозданного обьекта в списке, включен ли у него данные компоненты
            if (!Body[i].StickmanBody.gameObject.GetComponent<HingeJoint2D>().enabled ||
                 Body[i].StickmanAngle == null)
            {
                //если нет, то делаем неактивным этот элемент в списке
                Body[i].active = false;
            }
        }

        // проходимся от 0 до колличества состояний
        for (int k = 0; k < PlayerState.Count; k++)
        {
            // ищем пустое состояние. если оно пустое, то мы можем перезаписать его(или заполнить нужными данными)
            if (PlayerState[k].StateName == "")
            {
                // обнуляем список внутри состояния, которое отвечает за данные всех частей тела
                PlayerState[k].Part = new List<ClassPartState>();
                //проходимся от 0 до колличества частей тела, чтобы заполнить список part
                for (int i = 0; i < BodyPartsString.Length; i++)
                {
                    //добавляем новый обьект в список part
                    PlayerState[k].Part.Add(new ClassPartState
                    {
                        Name = BodyPartsString[i], //называем этот обьект
                        Part = gameObject.transform.Find(BodyPartsString[i]).gameObject, //находим для него gameobject
                        angle = 0, // по стандарту выставляем угол 0
                        force = DefaultForce, // выставляем силу по стандарту
                        speed = DefaultSpeed // выставляем скорость по стандарту
                    });
                }
            }
        }

    }*/

    /*[Button("SetStaticState")]
    public bool sd;
    public Transform sdf;
    public string sdffssss;*/
    #endregion

    #region Jumper
    public void SetToJumper(float time)
    {
        foreach(BodyParts part in Body)
        {
            part.Rigidbody.sharedMaterial = JumperMaterial;
        }

        if (SetToNormalStateCor != null)
            StopCoroutine(SetToNormalStateCor);

        SetToNormalStateCor = StartCoroutine(SetToNormalState(time));
    }

    IEnumerator SetToNormalState(float time)
    {
        yield return new WaitForSeconds(time);

        foreach (BodyParts part in Body)
        {
            part.Rigidbody.sharedMaterial = part.physicsMaterial;
        }
    }
    #endregion

    public void SetStaticState()
    {
        SetStaticState(defaultPosition, DefaultState);
    }

    public void SetStaticState(Transform transform, string poseName)
    {
        SetStaticState(transform.position, poseName);
    }

    public void SetStaticState(Vector3 position, string poseName)
    {
        //gameObject.transform.parent = transform;
#if UNITY_EDITOR
        Undo.RecordObject(gameObject.transform, "State changed");
#endif
        gameObject.transform.position = position;

        for (int i = 0; i < PlayerStaticStates.Count; i++)
        {
            if (poseName == PlayerStaticStates[i].PoseName)
            {
                for (int j = 0; j < Body.Count; j++)
                {
#if UNITY_EDITOR
                    Undo.RecordObject(Body[j].Rigidbody.transform, "State changed");
#endif
                    Body[j].Rigidbody.transform.localPosition =  PlayerStaticStates[i].BodyParts[j].Position;
                    Body[j].Rigidbody.transform.localRotation =  PlayerStaticStates[i].BodyParts[j].Rotation;
                    Body[j].Rigidbody.isKinematic = true;
                }

#if UNITY_EDITOR
                Undo.RecordObject(gameObject.transform, "State changed");
#endif
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

                CanMove = false;
                return;
            }
        }
    }

    public void RawCatch()
    {
        _iscatch = true;
    }
    public void RawUnCatch()
    {
        _iscatch = false;
    }

    public void Catch()
    {
        cath = true;
    }
    public void UnCatch()
    {
        cath = false;
    }
}
