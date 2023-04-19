using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegController : MonoBehaviour
{
    public HingeJoint2D RHip;
    public JointMotor2D RHipM;
    public HingeJoint2D LHip;
    public JointMotor2D LHipM;
    public HingeJoint2D RKnee;
    public JointMotor2D RKneeM;
    public HingeJoint2D LKnee;
    public JointMotor2D LKneeM;
    public HingeJoint2D RAnkle;
    public JointMotor2D RAnkleM;
    public HingeJoint2D LAnkle;
    public JointMotor2D LAnkleM;

    public Vector2 TargetPosR;
    public Vector2 TargetPosL;
    private float TargetDst;
    private float TargetAngleR;
    private float KneeTargetAngleR;
    private float TargetAngleL;
    private float KneeTargetAngleL;
    public float ThighLength;
    public float CalfLength;
    private float jointSpeed = 30;
    private float targetSpeed = 0.25f;
    private Rigidbody2D rigidbodyRef;

    public void Awake()
    {
        rigidbodyRef = GetComponent<Rigidbody2D>();
        TargetPosR = new Vector2(ThighLength, -CalfLength);
        TargetPosL = new Vector2(-CalfLength, -ThighLength);
    }

    //Set target angles
    private void setTargetKneeAngle(int leg) //0 for right, 1 for left
    {
        Vector2 TargetPos = Vector2.zero;
        if (leg == 0)
        {
            TargetPos = TargetPosR;
        } else
        {
            TargetPos = TargetPosL;
        }
        TargetDst = MathC.Hyp(TargetPos.x, TargetPos.y);
        if (TargetDst > ThighLength + CalfLength - 0.001f)
        {
            TargetDst = ThighLength + CalfLength - 0.001f;
        }
        float TargetAngle = Mathf.Rad2Deg * (Mathf.Atan2(-TargetPos.x, -TargetPos.y) + Mathf.PI/2 - MathC.CosLawAng(ThighLength, TargetDst, CalfLength));
        float KneeTargetAngle = 180 - MathC.CosLawAng(CalfLength, ThighLength, TargetDst) * Mathf.Rad2Deg;
        if(leg == 0)
        {
            TargetAngleR = TargetAngle;
            KneeTargetAngleR = KneeTargetAngle;
        } else
        {
            TargetAngleL = TargetAngle;
            KneeTargetAngleL = KneeTargetAngle;
        }
    }
    public void Update()
    {
        //manual movement of the 2 targets
        TargetPosR.x += Input.GetAxis("Horizontal") * Time.deltaTime * targetSpeed;
        TargetPosR.y += Input.GetAxis("Vertical") * Time.deltaTime * targetSpeed;
        TargetPosL.x += Input.GetAxis("Horizontal2") * Time.deltaTime * targetSpeed;
        TargetPosL.y += Input.GetAxis("Vertical2") * Time.deltaTime * targetSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) //move legs faster if shift is pressed
        {
            targetSpeed = 3;
            jointSpeed = 200;
        } else
        {
            targetSpeed = 1f;
            jointSpeed = 50;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            TargetPosR = new Vector2(ThighLength, -CalfLength);
            TargetPosL = new Vector2(-CalfLength, -ThighLength);
        }
        //correct pos make sure it doesn't get out of range
        setTargetKneeAngle(0);
        setTargetKneeAngle(1);
        if (MathC.Hyp(TargetPosR.x, TargetPosR.y) > CalfLength + ThighLength)
        {
            setTargetKneeAngle(0);
            TargetPosR = new Vector2(Mathf.Cos(TargetAngleR * Mathf.Deg2Rad) * (ThighLength + CalfLength - 0.05f), -Mathf.Sin(TargetAngleR * Mathf.Deg2Rad) * (ThighLength + CalfLength - 0.05f));
        }
        if (MathC.Hyp(TargetPosL.x, TargetPosL.y) > CalfLength + ThighLength)
        {
            setTargetKneeAngle(1);
            TargetPosL = new Vector2(Mathf.Cos(TargetAngleL * Mathf.Deg2Rad) * (ThighLength + CalfLength -0.05f), -Mathf.Sin(TargetAngleL * Mathf.Deg2Rad) * (ThighLength + CalfLength - 0.05f));
        }

        //apply torque to the main body to stand up
        rigidbodyRef.AddTorque(Mathf.Clamp(-transform.rotation.z*300, -8000,8000));

        //move the hips and knees to correct positions
        RHipM = new JointMotor2D { motorSpeed = -(RHip.jointAngle - TargetAngleR) / 15 * jointSpeed, maxMotorTorque = 100000000 };
        RHip.motor = RHipM;
        RKneeM = new JointMotor2D { motorSpeed = -(RKnee.jointAngle - KneeTargetAngleR) / 15 * jointSpeed, maxMotorTorque = 100000000 };
        RKnee.motor = RKneeM;
        LHipM = new JointMotor2D { motorSpeed = -(LHip.jointAngle - TargetAngleL) / 15 * jointSpeed, maxMotorTorque = 100000000 };
        LHip.motor = LHipM;
        LKneeM = new JointMotor2D { motorSpeed = -(LKnee.jointAngle - KneeTargetAngleL) / 15 * jointSpeed, maxMotorTorque = 100000000 };
        LKnee.motor = LKneeM;


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(TargetPosR + (Vector2)transform.position + Vector2.down*0.5f, 0.05f);
        Gizmos.DrawSphere(TargetPosL + (Vector2)transform.position + Vector2.down * 0.5f, 0.05f);
    }
}
