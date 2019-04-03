using UnityEngine;
using UnityEditor;
using System;

public class ForwardKinematics : ScriptableObject
{
   
    public void Transform(GameObject[] Cylinder, float[] rotationAngle)
    {
        float speed = 50F;
        float degree = 0;

        for (int i = 0; i < Cylinder.Length; i++)
        {
            if (i == 0 || i == 3 || i == 5)
            {
                if (rotationAngle[i] >= 0)
                    degree = Cylinder[i].transform.localEulerAngles.z;
                else
                    degree = Cylinder[i].transform.localEulerAngles.z - 360;

                if (Math.Abs(rotationAngle[i] - degree) > 2)
                {
                    Cylinder[i].transform.localRotation = Quaternion.Lerp(Cylinder[i].transform.localRotation,
                        Quaternion.Euler(0, 0, rotationAngle[i]), speed * Time.deltaTime / Quaternion.Angle(Cylinder[i].transform.localRotation, Quaternion.Euler(0, 0, rotationAngle[i])));
                }
                else
                {
                    Cylinder[i].transform.localRotation = Quaternion.Euler(0, 0, rotationAngle[i]);
                }
            }
            else
            {
                //
                if (rotationAngle[i] > 0)
                    degree = Cylinder[i].transform.localEulerAngles.y;
                else
                    degree = Cylinder[i].transform.localEulerAngles.y - 360;
                //
                if (Math.Abs(rotationAngle[i] - degree) > 2)
                {
                    Cylinder[i].transform.localRotation = Quaternion.Lerp(Cylinder[i].transform.localRotation,
                        Quaternion.Euler(0, rotationAngle[i], 0), speed * Time.deltaTime / Quaternion.Angle(Cylinder[i].transform.localRotation, Quaternion.Euler(0, rotationAngle[i], 0)));
                }
                else
                {
                    Cylinder[i].transform.localRotation = Quaternion.Euler(0, rotationAngle[i], 0);
                }
            }
        }
    }

}