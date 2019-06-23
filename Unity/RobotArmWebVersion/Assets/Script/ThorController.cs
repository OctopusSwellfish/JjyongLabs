using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;


public class HttpComm
{

    ThorController thorController;

    [Serializable]
    public class axisInfo
    {
        public float gripperValue;
        public float[] transform_value = new float[6];

    }

    public class testinfo
    {
        public float gripperValue;
        public double transform_value_0;
        public double transform_value_1;
        public double transform_value_2;
        public double transform_value_3;
        public double transform_value_4;
        public double transform_value_5;
    }

    public IEnumerator Recv()
    {
        thorController = GameObject.Find("Thor").GetComponent<ThorController>();

        UnityWebRequest www = UnityWebRequest.Get("http://54.180.39.228:3000/DB/data");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;

            string str = Encoding.Default.GetString(results);

            Debug.Log(str);


            str = str.Substring(0, str.Length - 1);
            str = str.Substring(1, str.Length - 1);


            testinfo test = JsonUtility.FromJson<testinfo>(str);

            thorController.RotationAxisAngles[0] = (float)test.transform_value_0;
            thorController.RotationAxisAngles[1] = (float)test.transform_value_1;
            thorController.RotationAxisAngles[2] = (float)test.transform_value_2;
            thorController.RotationAxisAngles[3] = (float)test.transform_value_3;
            thorController.RotationAxisAngles[4] = (float)test.transform_value_4;
            thorController.RotationAxisAngles[5] = (float)test.transform_value_5;
        }
    }
}

public class ThorController : MonoBehaviour
{

    public GameObject idlerGearR, pivotArmR, idlerGearAR, GripperR; // Right Gripper
    public GameObject ServoGearL, pivotArmL, ServoGearAL, GripperL; // Left Gripper

    public GameObject[] RotationAxes;   // Rotation Axis 1~6

    public float[] RotationAxisAngles = { 0, 0, 0, 0, 0, 0 };  // Angle of Rotation Axis 1~6
    private float GripperValue = 0;
    private HttpComm httpComm = new HttpComm();


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
        
        yield return new WaitForSeconds(0.05f);
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
        StartCoroutine(httpComm.Recv());

        /*
        for (int i = 0; i < RotationAxisAngles.Length; i++)
        {
            if (RotationAxisAngles[i] < 45)
            {
                RotationAxisAngles[i] = (float)(RotationAxisAngles[0] + 0.5);
                GripperValue = (float)(GripperValue + 0.5);
            }
        }
        */

        yield return new WaitForSeconds(1f);
        StartCoroutine(OnUpdatedValue());
    }
}
