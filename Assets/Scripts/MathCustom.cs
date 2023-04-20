using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathCustom
{
    //custom math functions

    public static float Hyp(float a, float b) //hypotenuse, for when I don't want to use Vector3.distance
    {
        return Mathf.Sqrt(Mathf.Pow(a,2) + Mathf.Pow(b,2));
    }
    public static float CosLawAng(float Add1, float Add2, float Opp) //Cosine Law, for solving angles
    {
        return Mathf.Acos((Mathf.Pow(Add1, 2) + Mathf.Pow(Add2, 2) - Mathf.Pow(Opp, 2)) / (2 * Add1 * Add2));
    }
    public static float CosLawOpp(float Add1, float Add2, float Angle) //Cosine Law, for solving for opposite side
    {
        return Mathf.Sqrt(Mathf.Pow(Add1, 2) + Mathf.Pow(Add2, 2) - (2 * Add1 * Add2 * Mathf.Cos(Angle)));
    }

}
