using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorController : MonoBehaviour
{

    public GameObject idlerGearR, pivotArmR, idlerGearAR, GripperR; // Right Gripper
    public GameObject ServoGearL, pivotArmL, ServoGearAL, GripperL; // Left Gripper

    public GameObject[] RotationAxes;   // Rotation Axis 1~6

    private int[] RotationAxisAngles = { 0, 0, 0, 0, 0, 0 };  // Angle of Rotation Axis 1~6
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
        for (int i = 0; i < RotationAxes.Length; i++)
        {
            if (i == 0 || i == 3 || i == 5)
            {
                RotationAxes[i].transform.localRotation = Quaternion.Euler(0, 0, RotationAxisAngles[i]);
            }
            else
            {
                RotationAxes[i].transform.localRotation = Quaternion.Euler(0, RotationAxisAngles[i], 0);
            }
        }
        
        yield return new WaitForSeconds(1f);
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
        // 

        yield return new WaitForSeconds(1f);
        StartCoroutine(OnUpdatedValue());
    }
}
