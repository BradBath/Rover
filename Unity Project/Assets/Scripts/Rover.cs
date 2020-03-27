using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Rover : MonoBehaviour
{
    [SerializeField] HingeJoint frontLeftWheel;
    [SerializeField] HingeJoint frontRightWheel;
    [SerializeField] HingeJoint frontLeftWheelBracket;
    [SerializeField] HingeJoint frontRightWheelBracket;

    [SerializeField] HingeJoint[] wheels;

    public static Rover instance;
    bool isDebug = false;
    Dictionary<string, Action<string[]>> commands = new Dictionary<string, Action<string[]>>()
    {
        {"move",(args)=>{
            instance.Move(args);
        }},
        {"turn",(args)=>{
            instance.Turn(args);
        }},
        {"stop",(args)=>{
            instance.Stop(args);
        }}
    };

    public void Stop(string[] args)
    {
        if (args.Length > 0)
        {
            print("Too many arguments. Expected 0, got "+args.Length.ToString());
            //Do error
            return;
        }
        foreach (HingeJoint wheel in wheels)
        {
            JointSpring newSpring = wheel.spring;
            newSpring.targetPosition = wheel.angle;
            wheel.spring = newSpring;
            wheel.useSpring = true;
            wheel.useMotor = false;
        }
    }

    void Turn(string[] args)
    {
        if (args.Length > 1)
        {
            print("Too many arguments. Expected 1, got"+args.Length.ToString());
            //Do error
            return;
        }
        float deg = 0;
        bool isArgFloat = float.TryParse(args[0], out deg);
        if (!isArgFloat)
        {
            print("Arg is not a float. Expected float, got "+args[0].GetType());
            //Do error
            return;
        }
        print("Turning " + deg.ToString() + " degrees");
        JointSpring newSpring = frontLeftWheelBracket.spring;
        newSpring.targetPosition = deg;
        frontLeftWheelBracket.spring = newSpring;
        frontRightWheelBracket.spring = newSpring;
    }

    public void Move(string[] args)
    {
        if (args.Length > 1)
        {
            print("Too many arguments. Expected 1, got" + args.Length.ToString());
            //Do error
            return;
        }
        float speed=0;
        bool isArgFloat = float.TryParse(args[0], out speed);
        if (!isArgFloat)
        {
            print("Arg is not a float. Expected float, got " + args[0].GetType());
            //Do error
            return;
        }
        foreach(HingeJoint wheel in wheels)
        {
            wheel.useSpring = false;
            wheel.useMotor = true;
            JointMotor newMotor = wheel.motor;
            newMotor.targetVelocity = speed;
            wheel.motor = newMotor;
        }
    }
    //IEnumerator TurnWheels(int deg)
    //{
    //    JointLimits newSpring = frontLeftWheelBracket.limits;
    //    newSpring.min = 0;
    //    newSpring.bounciness = 0;
    //    newSpring.bounceMinVelocity = 0;
    //    while (Mathf.Ceil(newSpring.max) != deg && Mathf.Ceil(newSpring.min) != -deg)
    //    {
            
    //        newSpring.max = Mathf.LerpUnclamped(newSpring.max,deg,0.5f*Time.deltaTime);
    //        newSpring.min = Mathf.LerpUnclamped(newSpring.min, deg,0.5f*Time.deltaTime);
    //        frontLeftWheelBracket.limits = newSpring;
    //        frontRightWheelBracket.limits = newSpring;

    //        yield return new WaitForEndOfFrame();
    //    }
    //}


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    public void DoCommand(Tuple<string,string[]> command)
    {
        print("Doing command: " + command.Item1);
        string[] args = command.Item2;
        string name = command.Item1;
        Action<string[]> commandAction;
        foreach(string arg in args)
        {
            print("Argument: " + arg);
        }
        if(commands.TryGetValue(name,out commandAction))
        {
            print("Found command.");
            commandAction(args);
        }
        else
        {
            //Do error
            return;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!isDebug)
            return;
        if (Input.GetButtonDown("Horizontal"))
        {
            Turn(new string[] { ((int)(25 * Input.GetAxisRaw("Horizontal"))).ToString() });
        }
        if (Input.GetButtonUp("Horizontal"))
        {
            Turn(new string[] { "0" });
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach(HingeJoint wheel in wheels)
            {
                JointSpring newSpring = wheel.spring;
                newSpring.targetPosition = wheel.angle;
                wheel.spring = newSpring;
                wheel.useSpring = true;
                wheel.useMotor = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (HingeJoint wheel in wheels)
            {
                wheel.useSpring = false;
                
                wheel.useMotor = true;
            }
        }
    }}
