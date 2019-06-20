using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorController : MonoBehaviour
{

    public GameObject idlerGearR, pivotArmR, idlerGearAR, GripperR; // Right Gripper
    public GameObject ServoGearL, pivotArmL, ServoGearAL, GripperL; // Left Gripper

    public GameObject[] RotationAxes;   // Rotation Axis 1~6

    private float[] RotationAxisAngles = { 0, 0, 0, 0, 0, 0 };  // Angle of Rotation Axis 1~6
    private int GripperValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OnRotatedAxis());
        StartCoroutine(OnGrip());
        StartCoroutine(OnUpdatedValue());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Rotate each rotation_axis of the robot arm
    IEnumerator OnRotatedAxis()
    {
        float speed = 25F;
        float degree = 0;
        for (int i = 0; i < RotationAxes.Length; i++)
        {
            if (i == 0 || i == 3 || i == 5)
            {

                if (RotationAxisAngles[i] >= 0)
                    degree = RotationAxes[i].transform.localEulerAngles.z;
                else
                    degree = RotationAxes[i].transform.localEulerAngles.z - 360;

                if (Math.Abs(RotationAxisAngles[i] - degree) > 2)
                {
                    RotationAxes[i].transform.localRotation = Quaternion.Lerp(RotationAxes[i].transform.localRotation,
                        Quaternion.Euler(0, 0, RotationAxisAngles[i]), speed * Time.deltaTime / Quaternion.Angle(RotationAxes[i].transform.localRotation, Quaternion.Euler(0, 0, RotationAxisAngles[i])));
                }
                else
                {
                    RotationAxes[i].transform.localRotation = Quaternion.Euler(0, 0, RotationAxisAngles[i]);
                }
            }
            else
            {
                //
                if (RotationAxisAngles[i] > 0)
                    degree = RotationAxes[i].transform.localEulerAngles.y;
                else
                    degree = RotationAxes[i].transform.localEulerAngles.y - 360;
                //
                if (Math.Abs(RotationAxisAngles[i] - degree) > 2)
                {
                    RotationAxes[i].transform.localRotation = Quaternion.Lerp(RotationAxes[i].transform.localRotation,
                        Quaternion.Euler(0, RotationAxisAngles[i], 0), speed * Time.deltaTime / Quaternion.Angle(RotationAxes[i].transform.localRotation, Quaternion.Euler(0, RotationAxisAngles[i], 0)));
                }
                else
                {
                    RotationAxes[i].transform.localRotation = Quaternion.Euler(0, RotationAxisAngles[i], 0);
                }
            }
        }
        
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(OnRotatedAxis());
    }

    // Grip a gripper
    // Rotate each gears, pivot arms and Transform Left, Right Gripper
    IEnumerator OnGrip()
    {
        // Move Right Gripper 
        idlerGearR.transform.localRotation = Quaternion.Euler(0, 0, GripperValue);
        pivotArmR.transform.localRotation = Quaternion.Euler(0, 0, GripperValue);
        GripperR.transform.position = idlerGearAR.transform.position;

        // Move Left Gripper 
        ServoGearL.transform.localRotation = Quaternion.Euler(0, 0, -GripperValue);
        pivotArmL.transform.localRotation = Quaternion.Euler(0, 0, -GripperValue);
        GripperL.transform.position = ServoGearAL.transform.position;

        yield return new WaitForSeconds(1f);
        StartCoroutine(OnGrip());
    }

    //
    IEnumerator OnUpdatedValue()
    {
        // Http Comm

        RotationAxisAngles[0] = (float)(RotationAxisAngles[0] + 0.1);
        RotationAxisAngles[1] = (float)(RotationAxisAngles[1] + 0.1);
        RotationAxisAngles[2] = (float)(RotationAxisAngles[2] + 0.1);
        RotationAxisAngles[3] = (float)(RotationAxisAngles[3] + 0.1);
        RotationAxisAngles[4] = (float)(RotationAxisAngles[4] + 0.1);
        RotationAxisAngles[5] = (float)(RotationAxisAngles[5] + 0.1);
        GripperValue++;

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(OnUpdatedValue());
    }
}
