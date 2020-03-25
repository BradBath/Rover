using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{

    [SerializeField] HingeJoint frontLeftWheel;
    [SerializeField] HingeJoint frontRightWheel;
    [SerializeField] Transform frontLeftWheelBracket;
    [SerializeField] Transform frontRightWheelBracket;


    public void Turn(float deg)
    {
        print("Turning " + deg.ToString() + " degrees");
        frontLeftWheelBracket.rotation = Quaternion.Euler(0, deg, 0);
        frontRightWheelBracket.rotation = Quaternion.Euler(0, deg, 0);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            Turn(75);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            Turn(-75);
        }
        else if (Input.GetAxis("Horizontal") == 0)
        {
            Turn(0);
        }
    }
}
