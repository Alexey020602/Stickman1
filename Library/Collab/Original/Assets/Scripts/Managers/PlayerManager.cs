using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


[System.Serializable]
public class BodyParts
{
    public string Name;
    public bool active;
    public Rigidbody2D Rigidbody;
    public StickmanBody StickmanBody;
    public StickmanAngle StickmanAngle;
    public float angle;
    public float force;
    public float speed;
}

[System.Serializable]
public class ClassPlayerState
{
    public string StateName;
    public List<ClassPartState> Part= new List<ClassPartState>();
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


public class PlayerManager : MonoBehaviour
{
    // inspector
    [Header("Body part")]
    public float DefaultSpeed = 100;
    public float DefaultForce = 5;
    public bool UseDefaultParameters = true;

    public string[] BodyPartsString = { "Head", "Body0", "Body1", "Body2", "LegForward1", "LegForward0", "LegBehind0", "LegBehind1", "HandBehind0", "HandBehind1", "HandForward0", "HandForward1" };

    // хранит в себе данные о всех частях тела
    public List<BodyParts> Body = new List<BodyParts>();

    [Header("Player Control")]
    public PlayerCling LeftHand;
    public PlayerCling RightHand;

    [Header("Player States")]
    public List<ClassPlayerState> PlayerState = new List<ClassPlayerState>();

    [Header("Game States")]
    public bool InfinityGame = false;

    [Header("Debug")]
    public string CurrentState;

    [Header ("Cling parameters")]
    private bool _iscatch = false;
    public float stamina = 1;
    public float hover_time;
    public float recovery_time;

    // managers
    private GameManager _gamemanager;
    private CanvasManager _canvasManager;
    private CountDownTimer _countDownTimer;

    // others
    private Rigidbody2D Head; // обьект для определения малой скорости, чтобы фиксировать конец игры
    private bool LowVelocityActive = true; // помогает нормально работать таймеру конца игры

    public enum BodyPartsName
    {
        Head,
        Body0,
        Body1,
        Body2,
        LegForward1,
        LegForward0,
        LegBehind0,
        LegBehind1,
        HandBehind0,
        HandBehind1,
        HandForward0,
        HandForward1
    }


    private void Start()
    {
        _countDownTimer = FindObjectOfType<CountDownTimer>();
        _canvasManager = FindObjectOfType<CanvasManager>();
        _gamemanager = FindObjectOfType<GameManager>();

        //при старте записываем обьект head, чтобы мы его могли использовать в дальнейшем для проверки конца игры
        Head = Body[1].Rigidbody;

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (_iscatch)
            {
                if (stamina >= 0.001)
                    Catch();
                else
                    UnCatch();
            }
            else
            {
                if (stamina >= 0.2)
                    Catch();
                else
                    UnCatch();
            }
        }
        else
        {
            UnCatch();
        }


        if (Input.GetKey("w"))
        {
            if (CurrentState != "Up")
            {
                SetControlOfBodyPart("Up");
            }
        }
        else if (Input.GetKey("a"))
        {
            if (CurrentState != "Front")
            {
                SetControlOfBodyPart("Front");
            }
        }
        else if (Input.GetKey("s"))
        {
            if (CurrentState != "Down")
            {
                SetControlOfBodyPart("Down");
            }
        }
        else if (Input.GetKey("d"))
        {
            if (CurrentState != "Back")
            {
                SetControlOfBodyPart("Back");
            }
        }
        else
        {
            DisabledControlOfBodyPart();
        }
    }


    private void FixedUpdate()
    {
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
        if (LeftHand.catched||RightHand.catched)
        {
            if(LeftHand.catched && RightHand.catched)
            { 
                stamina -=  Time.fixedTime /hover_time;
            
            }
            else
            {
                stamina -= 2*Time.fixedTime / hover_time;
            }
            if (stamina < 0) stamina = 0f;
        }
        else
        {
            stamina += Time.fixedTime / recovery_time;
            if (stamina > 1) stamina = 1f;
        }
    }

    // устанавливаем состояние персонажа
    public void SetControlOfBodyPart(string statename)
    {
        for (int i = 0; i < PlayerState.Count; i++)
        {
            if (PlayerState[i].StateName == statename)
            {
                CurrentState = statename;
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
            }
        }   
    }

    public void DisabledControlOfBodyPart()
    {
        CurrentState = null;
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
    public void SetImpulse(float Force)
    {
        Body[(int)BodyPartsName.Head].StickmanBody.StopGetDamage(0.1f);
        Body[(int)BodyPartsName.Head].Rigidbody.AddForce(new Vector2(-200 * Force - 20, 0), ForceMode2D.Impulse);
    }

    // проверяем скорость персонажа, чтобы закончить игру
    public void CheckLowVelocity()
    {
        if (InfinityGame) return;

        if (ModuleVector(Head.velocity) < 1f)
        {
            if (LowVelocityActive)
            {
                LowVelocityActive = false;
                _countDownTimer.ActiveCounter = true;
                _countDownTimer.timeLeft = 6f;

            }
        }
        else
        {
            LowVelocityActive = true;
            _countDownTimer.ActiveCounter = false;
        }
    }

    // функция для заполнения списков частями тел
    public void FindBodyesParts()
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
                 Body[i].StickmanAngle == null  )
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
                    }) ;
                }
            }
        }
        
    }

    public void Catch()
    {
        _iscatch = true;
    }
    public void UnCatch()
    {
        _iscatch = false;
    }



    public float ModuleVector(Vector2 Vector)
    {
        return math.sqrt(Vector.x * Vector.x + Vector.y * Vector.y);
    }
}
