using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegController : MonoBehaviour
{
    public HingeJoint2D RRHip;
    public HingeJoint2D LLHip;
    public HingeJoint2D RRKnee;
    public HingeJoint2D LLKnee;
    public HingeJoint2D RRAnkle;
    public HingeJoint2D LLAnkle;

    private float walkSpeed = 100f;
    public int walkPhase;
    public float walkWait;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Set variables
        var RHip = new JointMotor2D();
        var LHip = new JointMotor2D();
        var RKnee = new JointMotor2D();
        var LKnee = new JointMotor2D();
        var RAnkle = new JointMotor2D();
        var LAnkle = new JointMotor2D();
        RHip.maxMotorTorque = 1000f;
        LHip.maxMotorTorque = 1000f;
        RKnee.maxMotorTorque = 1000f;
        LKnee.maxMotorTorque = 1000f;
        RAnkle.maxMotorTorque = 10000f;
        LAnkle.maxMotorTorque = 10000f;

        //Lmfao walking moment
        if (walkPhase == 1)
        {
            
            RHip.motorSpeed = walkSpeed*1.75f;
            LHip.motorSpeed = -walkSpeed*1.5f;
            RKnee.motorSpeed = -walkSpeed*1.5f;
            LKnee.motorSpeed = walkSpeed;
            RAnkle.motorSpeed = walkSpeed*1.5f;
            LAnkle.motorSpeed = -walkSpeed/1.5f;
        }else if (walkPhase == 2)
        {
            RHip.motorSpeed = -walkSpeed*1.5f;
            LHip.motorSpeed = walkSpeed*1.75f;
            RKnee.motorSpeed = walkSpeed;
            LKnee.motorSpeed = -walkSpeed*1.5f;
            RAnkle.motorSpeed = -walkSpeed/1.5f;
            LAnkle.motorSpeed = walkSpeed*1.5f;
        } else if (walkPhase == -1)
        {

            RHip.motorSpeed = -walkSpeed * 1.75f;
            LHip.motorSpeed = walkSpeed * 1.5f;
            RKnee.motorSpeed = walkSpeed * 1.5f;
            LKnee.motorSpeed = -walkSpeed;
            RAnkle.motorSpeed = -walkSpeed * 1.5f;
            LAnkle.motorSpeed = walkSpeed / 1.5f;
        }
        else if (walkPhase == -2)
        {
            RHip.motorSpeed = walkSpeed * 1.5f;
            LHip.motorSpeed = -walkSpeed * 1.75f;
            RKnee.motorSpeed = -walkSpeed;
            LKnee.motorSpeed = walkSpeed * 1.5f;
            RAnkle.motorSpeed = walkSpeed / 1.5f;
            LAnkle.motorSpeed = -walkSpeed * 1.5f;
        }

        //Set all of the speed things
        LLHip.motor = LHip;
        RRHip.motor = RHip;
        LLKnee.motor = LKnee;
        RRKnee.motor = RKnee;
        RRAnkle.motor = RAnkle;
        LLAnkle.motor = LAnkle;

        //Input stuff, do walk cycle if a or d
        if (Input.GetKey(KeyCode.D))
        {
            if (walkWait >= 0 && walkWait < 3)
            {
                walkPhase = 1;
            }
            if (walkWait >= 3)
            {
                walkPhase = 2;
            }
            if (walkWait > 6)
            {
                walkWait = 0;
            }
            walkWait = walkWait + Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (walkWait >= 0 && walkWait < 3)
            {
                walkPhase = -1;
            }
            if (walkWait >= 3)
            {
                walkPhase = -2;
            }
            if (walkWait > 6)
            {
                walkWait = 0;
            }
            walkWait = walkWait + Time.deltaTime;
        }
        else
        {
            walkWait = 0;
            walkPhase = 1;
        }

        var RRRHip = RRHip.limits;
        var LLLHip = LLHip.limits;
        var RRRKnee = RRKnee.limits;
        var LLLKnee = LLKnee.limits;
        var RRRAnkle = RRAnkle.limits;
        var LLLAnkle = LLAnkle.limits;
        //flip if input
        if (Input.GetKeyDown(KeyCode.D))
        {
            RRRHip.min = -90;
            RRRHip.max = 66;
            LLLHip.min = -90;
            LLLHip.max = 66;
            RRRKnee.min = 80;
            RRRKnee.max = 0;
            LLLKnee.min = 80;
            LLLKnee.max = 0;
            RRRAnkle.min = 80;
            RRRAnkle.max = -10;
            LLLAnkle.min = 80;
            LLLAnkle.max = -10;
        } else if (Input.GetKeyDown(KeyCode.A))
        {
            RRRHip.min = -90;
            RRRHip.max = 66;
            LLLHip.min = -90;
            LLLHip.max = 66;
            RRRKnee.min = 80;
            RRRKnee.max = 0;
            LLLKnee.min = 80;
            LLLKnee.max = 0;
            RRRAnkle.min = 10;
            RRRAnkle.max = -80;
            LLLAnkle.min = 10;
            LLLAnkle.max = -80;
        }
        RRHip.limits = RRRHip;
        LLHip.limits = LLLHip;
        RRKnee.limits = RRRKnee;
        LLKnee.limits = LLLKnee;
        RRAnkle.limits = RRRAnkle;
        LLAnkle.limits = LLLAnkle;


    }
}
