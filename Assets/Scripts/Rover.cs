using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{

    [SerializeField] HingeJoint frontLeftWheel;
    [SerializeField] HingeJoint frontRightWheel;
    [SerializeField] HingeJoint frontLeftWheelBracket;
    [SerializeField] HingeJoint frontRightWheelBracket;


    public void Turn(float deg)
    {
        print("Turning " + deg.ToString() + " degrees");
        JointLimits newLimits = frontLeftWheelBracket.limits;
        newLimits.min = 0;
        newLimits.bounciness = 0;
        newLimits.bounceMinVelocity = 0;
        newLimits.max = deg;
        newLimits.min = -deg;
        frontLeftWheelBracket.limits = newLimits;
        frontRightWheelBracket.limits = newLimits;
    }

    IEnumerator TurnWheels()
    {
        yield return null;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            Turn(25);
        }
        if (Input.GetButtonUp("Horizontal"))
        {
            Turn(0);
        }

    }
}
