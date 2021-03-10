using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanAngle : MonoBehaviour
{
    // наименьший угол отклонения
    public float LowerAngle;
    // наибольший угол отклонения
    public float UpperAngle;
    // текущий угол отклонения
    public float JointAngle;

    // сила мотора для предельного угла.
    // чем больше это значение, тем больше сила, которая будет пытаться повернуть часть тела из предельного угла
    public float ForceMotor = 5;
    // скорость мотора для предельного угла.
    // чем больше это значение, тем больше скорость, которая будет пытаться повернуть часть тела из предельного угла
    public float motorSpeed = 200;

    public float ConForce;
    public float ConSpeed;
    public float ConAngle;

    public bool IsControl;

    private HingeJoint2D _joint2D;



    void Start()
    {
        _joint2D = GetComponent<HingeJoint2D>();
        JointMotor2D motor = _joint2D.motor;
        LowerAngle = _joint2D.limits.max;
        UpperAngle = _joint2D.limits.min;
        motor.motorSpeed = motorSpeed;
        _joint2D.motor = motor;
    }


    public void FixedUpdate()
    {
        if(_joint2D == null)
        {
            GetComponent<StickmanAngle>().enabled = false;
            return;
        }


        JointAngle = _joint2D.jointAngle;

        // проверяем, упелось ли часть тела в предельный угол
        if (CheckEndAngles()) return;

        // часть тела не уперлась в предельный угол. управляем этой частью тела
        if(IsControl)  AngleControl();

    }
    public bool CheckEndAngles()
    {
        JointMotor2D motor = _joint2D.motor;
        if (JointAngle < UpperAngle)
        {
            motor.motorSpeed = motorSpeed;
            motor.maxMotorTorque = (UpperAngle - _joint2D.jointAngle) * ForceMotor * ForceMotor;
            _joint2D.motor = motor;
            return true;
        }
        else if (JointAngle > LowerAngle)
        {
            motor.motorSpeed = -motorSpeed;
            motor.maxMotorTorque = (_joint2D.jointAngle - LowerAngle) * ForceMotor * ForceMotor;
            _joint2D.motor = motor;
            return true;
        }
        else
        {
            motor.maxMotorTorque = ForceMotor;
            motor.motorSpeed = 0;
        }
        _joint2D.motor = motor;
        return false;
    }

    // эта функция управляет частью тела
    public void AngleControl()
    {
        JointMotor2D motor = _joint2D.motor;
        if (JointAngle < ConAngle - 0.5f)
        {
            motor.motorSpeed =( ConSpeed * Mathf.Pow((JointAngle + 2 - ConAngle), 2))/50;
            motor.maxMotorTorque = ConForce ;
        }
        else if (JointAngle > ConAngle + 0.5f)
        {
            motor.motorSpeed = (-ConSpeed * Mathf.Pow((JointAngle - 2  - ConAngle), 2))/50;
            motor.maxMotorTorque = ConForce;
        }
        else
        {
            motor.motorSpeed = 0;
            motor.maxMotorTorque = ConForce*5;
        }
        _joint2D.motor = motor;
    }
}
