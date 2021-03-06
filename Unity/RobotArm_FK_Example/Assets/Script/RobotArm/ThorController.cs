using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Networking;

public class StoredClass

{

    public String result;
    public float gripperValue;
    public float[] transform_value = new float[6];

    public void Send()
    {
        string json_send = JsonUtility.ToJson(this);
        UnityWebRequest www = UnityWebRequest.Put("http://54.180.39.228:3000/DB/", json_send);
        Debug.Log(json_send);

        www.SetRequestHeader("Content-Type", "application/json");
        www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}

public class StepClass
{

    public int step;
    public void Send()
    {
        string json_send = JsonUtility.ToJson(this);
        UnityWebRequest www = UnityWebRequest.Put("http://54.180.39.228:3000/DB/scene", json_send);
        Debug.Log(json_send);

        www.SetRequestHeader("Content-Type", "application/json");
        www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}

public class ThorController : MonoBehaviour
{
    StoredClass storedclass = new StoredClass();
    StepClass stepClass = new StepClass();

    public GameObject[] Cylinder = new GameObject[6];
    public Slider[] ArtSlider = new Slider[6];
    public Slider GripperSlider;

    public GameObject gripper;
    public GameObject commGameObject;

    public Slider KinematicsSlider;
    public static double[] theta = new double[6];    //angle of the joints

    private bool FKinematics = true;
    private bool isMovement = true;
    private GameObject mCube;
    private bool gripperCheck = false;

    private bool mStop = false;
    private bool mMoveArtCorutine = false;
    private bool mUndoTest1 = false;

    private bool bTestStart = false;

    //
    private bool bTestRuning = false;
    private bool bUnTestRuning = false;


    private int step = 0;

    private GripperController gripperController;
    private CommController commController;

    private float L1, L2, L3, L4, L5, L6;    //arm length in order from base
    private float C3;

    private float px = 8f, py = 0f, pz = 8f;
    private float rx = 0f, ry = 0f, rz = 0f;


    // Use this for initialization
    void Start()
    {
        theta[0] = theta[1] = theta[2] = theta[3] = theta[4] = theta[5] = 0.0;
        L1 = 4.0f;
        L2 = 6.0f;
        L3 = 3.0f;
        L4 = 4.0f;
        L5 = 2.0f;
        L6 = 1.0f;
        C3 = 0.0f;

        gripperController = gripper.GetComponent<GripperController>();
        commController = commGameObject.GetComponent<CommController>();
        mCube = GameObject.Find("Cube");

        StartCoroutine("MoveArt");
        StartCoroutine("isMovementCheck");
        StartCoroutine("CollsionCheck");
        StartCoroutine("SendWeb");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator MoveArt()
    {
        mMoveArtCorutine = false;
        if (FKinematics)
        {
            float speed = 25F;
            float degree = 0;

            for (int i = 0; i < Cylinder.Length; i++)
            {
                if (i == 0 || i == 3 || i == 5)
                {

                    if (ArtSlider[i].value >= 0)
                        degree = Cylinder[i].transform.localEulerAngles.z;
                    else
                        degree = Cylinder[i].transform.localEulerAngles.z - 360;

                    if (Math.Abs(ArtSlider[i].value - degree) > 2)
                    {
                        Cylinder[i].transform.localRotation = Quaternion.Lerp(Cylinder[i].transform.localRotation,
                            Quaternion.Euler(0, 0, ArtSlider[i].value), speed * Time.deltaTime / Quaternion.Angle(Cylinder[i].transform.localRotation, Quaternion.Euler(0, 0, ArtSlider[i].value)));
                    }
                    else
                    {
                        Cylinder[i].transform.localRotation = Quaternion.Euler(0, 0, ArtSlider[i].value);
                    }
                }
                else
                {
                    //
                    if (ArtSlider[i].value > 0)
                        degree = Cylinder[i].transform.localEulerAngles.y;
                    else
                        degree = Cylinder[i].transform.localEulerAngles.y - 360;
                    //
                    if (Math.Abs(ArtSlider[i].value - degree) > 2)
                    {
                        Cylinder[i].transform.localRotation = Quaternion.Lerp(Cylinder[i].transform.localRotation,
                            Quaternion.Euler(0, ArtSlider[i].value, 0), speed * Time.deltaTime / Quaternion.Angle(Cylinder[i].transform.localRotation, Quaternion.Euler(0, ArtSlider[i].value, 0)));
                    }
                    else
                    {
                        Cylinder[i].transform.localRotation = Quaternion.Euler(0, ArtSlider[i].value, 0);
                    }
                }
            }
        }
        else
        {
            float ax, ay, az, bx, by, bz;
            float asx, asy, asz, bsx, bsy, bsz;
            float p5x, p5y, p5z;
            float C1, C23, S1, S23;

            px = ArtSlider[0].value;
            py = ArtSlider[1].value;
            pz = ArtSlider[2].value;
            rx = ArtSlider[3].value;
            ry = ArtSlider[4].value;
            rz = ArtSlider[5].value;

            ax = Mathf.Cos(rz * 3.14f / 180.0f) * Mathf.Cos(ry * 3.14f / 180.0f);
            ay = Mathf.Sin(rz * 3.14f / 180.0f) * Mathf.Cos(ry * 3.14f / 180.0f);
            az = -Mathf.Sin(ry * 3.14f / 180.0f);

            p5x = px - (L5 + L6) * ax;
            p5y = py - (L5 + L6) * ay;
            p5z = pz - (L5 + L6) * az;

            theta[0] = Mathf.Atan2(p5y, p5x);

            C3 = (Mathf.Pow(p5x, 2) + Mathf.Pow(p5y, 2) + Mathf.Pow(p5z - L1, 2) - Mathf.Pow(L2, 2) - Mathf.Pow(L3 + L4, 2)) / (2 * L2 * (L3 + L4));
            theta[2] = Mathf.Atan2(Mathf.Pow(1 - Mathf.Pow(C3, 2), 0.5f), C3);

            float M = L2 + (L3 + L4) * C3;
            float N = (L3 + L4) * Mathf.Sin((float)theta[2]);
            float A = Mathf.Pow(p5x * p5x + p5y * p5y, 0.5f);
            float B = p5z - L1;
            theta[1] = Mathf.Atan2(M * A - N * B, N * A + M * B);

            C1 = Mathf.Cos((float)theta[0]);
            C23 = Mathf.Cos((float)theta[1] + (float)theta[2]);
            S1 = Mathf.Sin((float)theta[0]);
            S23 = Mathf.Sin((float)theta[1] + (float)theta[2]);

            bx = Mathf.Cos(rx * 3.14f / 180.0f) * Mathf.Sin(ry * 3.14f / 180.0f) * Mathf.Cos(rz * 3.14f / 180.0f) - Mathf.Sin(rx * 3.14f / 180.0f) * Mathf.Sin(rz * 3.14f / 180.0f);
            by = Mathf.Cos(rx * 3.14f / 180.0f) * Mathf.Sin(ry * 3.14f / 180.0f) * Mathf.Sin(rz * 3.14f / 180.0f) - Mathf.Sin(rx * 3.14f / 180.0f) * Mathf.Cos(rz * 3.14f / 180.0f);
            bz = Mathf.Cos(rx * 3.14f / 180.0f) * Mathf.Cos(ry * 3.14f / 180.0f);

            asx = C23 * (C1 * ax + S1 * ay) - S23 * az;
            asy = -S1 * ax + C1 * ay;
            asz = S23 * (C1 * ax + S1 * ay) + C23 * az;
            bsx = C23 * (C1 * bx + S1 * by) - S23 * bz;
            bsy = -S1 * bx + C1 * by;
            bsz = S23 * (C1 * bx + S1 * by) + C23 * bz;

            theta[3] = Mathf.Atan2(asy, asx);
            theta[4] = Mathf.Atan2(Mathf.Cos((float)theta[3]) * asx + Mathf.Sin((float)theta[3]) * asy, asz);
            theta[5] = Mathf.Atan2(Mathf.Cos((float)theta[3]) * bsy - Mathf.Sin((float)theta[3]) * bsx, -bsz / Mathf.Sin((float)theta[4]));

            if (!double.IsNaN(theta[0]))
                Cylinder[0].transform.localEulerAngles = new Vector3(0, 0, (float)theta[0] * Mathf.Rad2Deg);

            if (!double.IsNaN(theta[1]))
                Cylinder[1].transform.localEulerAngles = new Vector3(0, (float)theta[1] * Mathf.Rad2Deg, 0);
            if (!double.IsNaN(theta[2]))
                Cylinder[2].transform.localEulerAngles = new Vector3(0, (float)theta[2] * Mathf.Rad2Deg, 0);
            if (!double.IsNaN(theta[3]))
                Cylinder[3].transform.localEulerAngles = new Vector3(0, 0, (float)theta[3] * Mathf.Rad2Deg);
            if (!double.IsNaN(theta[4]))
                Cylinder[4].transform.localEulerAngles = new Vector3(0, (float)theta[4] * Mathf.Rad2Deg, 0);
            if (!double.IsNaN(theta[5]))
                Cylinder[5].transform.localEulerAngles = new Vector3(0, 0, (float)theta[5] * Mathf.Rad2Deg);



        }

        yield return new WaitForSeconds(0.01f);
        if (!mStop)
        {

            StartCoroutine(MoveArt());
        }
        else
        {
            mMoveArtCorutine = true;
        }
    }

    public void OnChangedKinematics()
    {

        switch ((int)KinematicsSlider.value)
        {
            case 0:
                FKinematics = true;
                break;
            case 1:
                FKinematics = false;
                break;

        }
    }

    public void TransformValue(float value1, float value2, float value3, float value4, float value5, float value6)
    {
        ArtSlider[0].value = value1;
        ArtSlider[1].value = value2;
        ArtSlider[2].value = value3;
        ArtSlider[3].value = value4;
        ArtSlider[4].value = value5;
        ArtSlider[5].value = value6;
    }


    IEnumerator isMovementCheck()
    {
        bool result = true;
        int degree;

        for (int i = 0; i < ArtSlider.Length; i++)
        {
            if (i == 0 || i == 3 || i == 5)
            {
                if (ArtSlider[i].value >= 0)
                    degree = (int)Cylinder[i].transform.localEulerAngles.z;
                else
                    degree = (int)Cylinder[i].transform.localEulerAngles.z - 360;

                if ((int)ArtSlider[i].value == degree)
                    result = result && true;
                else
                    result = result && false;
            }
            else
            {
                if (ArtSlider[i].value >= 0)
                    degree = (int)Cylinder[i].transform.localEulerAngles.y;
                else
                    degree = (int)Cylinder[i].transform.localEulerAngles.y - 360;

                if ((int)ArtSlider[i].value == degree)
                    result = result && true;
                else
                    result = result && false;
            }
        }

        isMovement = result;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(isMovementCheck());

    }

    public void OnClickTest()
    {
        bTestStart = true;
        if (bTestRuning == false)
            StartCoroutine("Test1");

    }

    IEnumerator GrippedObject()
    {
        if (gripperController.getMaxValue() > GripperSlider.value + 1)
        {
            //.Log("" + gripperController.getMaxValue());
            //GripperSlider.value++;
            GripperSlider.value += 5; //그리퍼 속도 수정( 실제 그리퍼 속도가 빨라서 실제 그리퍼 속도랑 동기화 하였습니다. )
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(GrippedObject());
        }
        else
        {
            gripperCheck = true;
            StopCoroutine(GrippedObject());
        }
    }

    IEnumerator UngrippedObject()
    {

        if (GripperSlider.value > 0)
        {
            GripperSlider.value -= 5; //그리퍼 속도 수정( 실제 그리퍼 속도가 빨라서 실제 그리퍼 속도랑 동기화 하였습니다. )
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(UngrippedObject());
        }
        else
        {
            gripperCheck = true;
            StopCoroutine(UngrippedObject());
        }
    }

    IEnumerator Test1()
    {
        bTestRuning = true;

        /*
        yield return new WaitWhile(() => isMovement != true);
        // Step 1
        if (step <= 0)
        {
            //commController.send("G0 A10 B0 C0 D10 X0 Y5 Z5 \n");
            TransformValue(0, 60, 75, 0, 0, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
            //yield return new WaitWhile(() => commController.FristRecv != true);
            step = 1;
        }
        // Step 2 - Gripped object
        if (step <= 1)
        {
            StartCoroutine(GrippedObject());
            yield return new WaitWhile(() => gripperCheck != true);
            yield return new WaitForSeconds(1f);
            gripperCheck = false;
            step = 2;
        }
        // Step 3
        if (step <= 2)
        {
            //commController.send("G0 A0 B25 C60 X0 Y0 Z0 \n");
            TransformValue(0, 25, 60, 0, 0, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
            //yield return new WaitWhile(() => commController.FristRecv != true);
            step = 3;
        }
        // Step 4
        if (step <= 3)
        {
            //commController.send("G0 A-135 B25 C60 X0 Y0 Z0 \n");
            TransformValue(-135, 25, 60, 0, 0, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
            step = 4;
        }
        // Step 5 - Ungirpped object
        if (step <= 4)
        {
            StartCoroutine(UngrippedObject());
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => gripperCheck != true);
            gripperCheck = false;
            step = 5;
        }
        */
        /*
        //
        if (step <= 0)
        {
            TransformValue(0, 0, 0, 0, 0, 0);
            step = 1;
        }
        if (step <= 1)
        {
            yield return new WaitForSeconds(2f);
            TransformValue(0, 0, 0, -90, 0, 0);
            yield return new WaitForSeconds(2f);

            step = 2;
        }
        if (step <= 2)
        {
            yield return new WaitForSeconds(2f);
            TransformValue(0, 20 , 0, -90, 0,0);
            yield return new WaitForSeconds(2f);

            step = 3;
        }
        if (step <= 3)
        {
            yield return new WaitForSeconds(2f);
            TransformValue(0, 20, 110, -90, 50, 0);
            yield return new WaitForSeconds(2f);

            step = 4;
        }
        if (step <= 4)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(GrippedObject());
            yield return new WaitForSeconds(2f);

            step = 5;
        }
        if (step <= 5)
        {
            yield return new WaitForSeconds(2f);
            TransformValue(90, 20, 110, -90, 50, 0);
            yield return new WaitForSeconds(2f);

            step = 6;
        }
        if (step <= 6)
        {
            yield return new WaitForSeconds(2f);
            TransformValue(180, 20, 110, -90, 50, 0);
            yield return new WaitForSeconds(2f);

            step = 7;
        }
        if (step <= 7)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(UngrippedObject());
            yield return new WaitForSeconds(2f);

            step = 8;
        }
        if (step <= 8)
        {
            yield return new WaitForSeconds(2f);
            TransformValue(180, 20, 0, -90, 0, 0);
            yield return new WaitForSeconds(2f);

            step = 9;
        }
        if (step <= 9)
        {
            yield return new WaitForSeconds(2f);
            TransformValue(180, 20, 0, -90, 0, 0);
            yield return new WaitForSeconds(2f);

            step = 10;
        }
        if (step <= 10)
        {
            yield return new WaitForSeconds(2f);
            TransformValue(0, 0, 0, 0, 0, 0);
            yield return new WaitForSeconds(2f);

            step = 11;
        }



        */
        ////////////////////////////////////////////////////////////////
        // Step 1
        if (step <= 0)
        {
            TransformValue(0, 0, 0, 0, 0, 0);
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => isMovement != true);
            commController.send("G0 A0 B0 C0 D0 X0 Y0 Z0 \n");
            //yield return new WaitWhile(() => commController.FristRecv != true);
            yield return new WaitForSeconds(1f);

            step = 1;
            stepClass.step = step;
            //stepClass.Send();
        }

        // Step 2 TransformValue(0, 0, 0, -90, 0, 0);
        if (step <= 1)
        {
            TransformValue(0, 0, 0, -90, 0, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
            commController.send("G0 A0 B0 C0 D0 X6.5 Y0 Z0 \n");
            //yield return new WaitWhile(() => commController.FristRecv != true);
            yield return new WaitForSeconds(2f);
            step = 2;
            stepClass.step = step;
            //stepClass.Send();
        }
        // Step 3  TransformValue(0, 20 , 0, -90, 0,0);
        if (step <= 2)
        {
            TransformValue(0, 20, 0, -90, 0, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
            commController.send("G0 A0 B20 C20 D0 X6.5 Y0 Z0 \n");
           // yield return new WaitWhile(() => commController.FristRecv != true);
            yield return new WaitForSeconds(2f);
            step = 3;
            stepClass.step = step;
           // stepClass.Send();
        }
        // Step 4
        if (step <= 3)
        {
            TransformValue(0, 20, 110, -90, 50, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
            commController.send("G0 A0 B20 C20 D110 X6.5 Y-2.5 Z2.5 \n");
            //yield return new WaitWhile(() => commController.FristRecv != true);
            yield return new WaitForSeconds(2f);
            step = 4;
            stepClass.step = step;
           // stepClass.Send();
        }
        // Step 5   Gripper
        if (step <= 4)
        {
            StartCoroutine(GrippedObject());
            yield return new WaitWhile(() => gripperCheck != true);
            yield return new WaitForSeconds(2f);
            gripperCheck = false;
            commController.send("M3 S0 \n");
          //  yield return new WaitWhile(() => commController.FristRecv != true);
            yield return new WaitForSeconds(2f);
            step = 5;
            stepClass.step = step;
           // stepClass.Send();
        }
        // Step 6 90, 20, 110, -90, 50, 0);
        if (step <= 5)
        {
            TransformValue(90, 20, 110, -90, 50, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
            commController.send("G0 A17 B20 C20 D110 X6.5 Y-2.5 Z2.5 \n");
           // yield return new WaitWhile(() => commController.FristRecv != true);
            yield return new WaitForSeconds(2f);
            step = 6;
            stepClass.step = step;

           // stepClass.Send();
        }
        // Step 7 (180, 20, 110, -90, 50, 0);
        if (step <= 6)
        {
            TransformValue(180, 20, 110, -90, 50, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
            commController.send("G0 A34 B20 C20 D110 X6.5 Y-2.5 Z2.5 \n");
           // yield return new WaitWhile(() => commController.FristRecv != true);
            yield return new WaitForSeconds(2f);
            step = 7;
            stepClass.step = step;
          //  stepClass.Send();
        }


        // Step 8 Ungripper
        if (step <= 7)
        {

            StartCoroutine(UngrippedObject());
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => gripperCheck != true);
            gripperCheck = false;
            commController.send("M3 S1000 \n");
           // yield return new WaitWhile(() => commController.FristRecv != true);
            yield return new WaitForSeconds(2f);
            step = 8;
            stepClass.step = step;
           // stepClass.Send();
        }

        // Step 9 (180, 20, 110, -90, 50, 0);
        if (step <= 8)
        {
            TransformValue(180, 20, 0, -90, 0, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
            commController.send("G0 A34 B20 C20 D0 X6.5 Y0 Z0 \n");
           // yield return new WaitWhile(() => commController.FristRecv != true);
            yield return new WaitForSeconds(2f);
            step = 9;
            stepClass.step = step;
           // stepClass.Send();
        }
        // Step 10 (180, 20, 110, -90, 50, 0);
        if (step <= 9)
        {
            TransformValue(0, 0, 0, 0, 0, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
            commController.send("G0 A0 B0 C0 D0 X0 Y0 Z0 \n");
           //yield return new WaitWhile(() => commController.FristRecv != true);
            yield return new WaitForSeconds(2f);
            step = 10;
            stepClass.step = step;
           // stepClass.Send();
        }
        /////////////////////////////////////////////////////////////////////////////
        ///그리퍼 움직이려면 StartCoroutine("GrippedObject"); 쓰면됨
        ///물기 전까지  ++됩니다.
        ////////////////////////////////////////////////////////////////////////////
        bTestRuning = false;
    }

    IEnumerator unTest1()
    {

        bUnTestRuning = true;
        /*
        if (step == 5)
        {
            StartCoroutine(GrippedObject());
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => gripperCheck != true);
            gripperCheck = false;
        }
        if (step == 4)
        {
            TransformValue(-135, 25, 60, 0, 0, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
        }
        if (step == 3)
        {
            TransformValue(0, 25, 60, 0, 0, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
        }
        if (step == 2)
        {
            TransformValue(0, 60, 75, 0, 0, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
        }
        if (step == 1)
        {
            StartCoroutine(UngrippedObject());
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => gripperCheck != true);
            gripperCheck = false;
        }

        if (step == 0)
        {
            TransformValue(0, 0, 0, 0, 0, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
        }*/

        if (step == 10)
        {
            TransformValue(0, 0, 0, 0, 0, 0);
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => isMovement != true);
        }
        if (step == 9)
        {
            TransformValue(180, 20, 0, -90, 0, 0);
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => isMovement != true);
        }
        if (step == 8)
        {
            TransformValue(180, 20, 110, -90, 50, 0);
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => isMovement != true);
            /*StartCoroutine(UngrippedObject());
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => gripperCheck != true);
            gripperCheck = false;*/ 
        
        }
        if (step == 7)
        {
            TransformValue(180, 20, 110, -90, 50, 0);
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => isMovement != true);
        }
        if (step == 6)
        {
            TransformValue(90, 20, 110, -90, 50, 0);
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => isMovement != true);
        }
        if (step == 5)
        {
            TransformValue(0, 20, 110, -90, 50, 0);
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => isMovement != true);
            /*StartCoroutine(GrippedObject());
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => gripperCheck != true);
            gripperCheck = false;*/
               
        }
        if (step == 4)
        {
            TransformValue(0, 20, 110, -90, 50, 0);
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => isMovement != true);
        }
        if (step == 3)
        {
            TransformValue(0, 20, 0, -90, 0, 0);
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => isMovement != true);
        }

        if (step == 2)
        {
            TransformValue(0, 0, 0, -90, 0, 0);
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => isMovement != true);
        }
        if (step == 1)
        {
            TransformValue(0, 0, 0, 0, 0, 0);
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => isMovement != true);
        }
        if(bTestRuning == false)
            StartCoroutine("Test1");
        bUnTestRuning = false;
    }


    IEnumerator CollsionCheck()
    {

        if (Collison.instance.SphereCheck())
        {
            mStop = true;

            storedclass.result = "true";
            //storedclass.Send();

            StopCoroutine("Test1");
            bTestRuning = false;
            StopCoroutine("GrippedObject");
            StopCoroutine("UngrippedObject");
            
            if (!mUndoTest1 && bTestStart)
            {
                //test2();
                if(bUnTestRuning == false)
                    StartCoroutine("unTest1");
                mUndoTest1 = true;
            }

        }
        else
        {
            mUndoTest1 = false;
            mStop = false;

            storedclass.result = "false";
            // storedclass.Send();

            if (mMoveArtCorutine)
                StartCoroutine("MoveArt");
        }
        yield return new WaitForSeconds(0.01f);

        StartCoroutine("CollsionCheck");

    }

    public void test2()
    {
        float degree = 0;


        for (int i = 0; i < Cylinder.Length; i++)
        {
            if (i == 0 || i == 3 || i == 5)
            {
                if (ArtSlider[i].value >= 0)
                    degree = (int)Cylinder[i].transform.localEulerAngles.z;
                else
                    degree = (int)Cylinder[i].transform.localEulerAngles.z - 360;
                ArtSlider[i].value = degree;

            }
            else
            {
                if (ArtSlider[i].value >= 0)
                    degree = (int)Cylinder[i].transform.localEulerAngles.y;
                else
                    degree = (int)Cylinder[i].transform.localEulerAngles.y - 360;
                ArtSlider[i].value = degree;
            }
        }

    }

    IEnumerator SendWeb()
    {
        float degree = 0;

        for (int i = 0; i < Cylinder.Length; i++)
        {
            if (i == 0 || i == 3 || i == 5)
            {
                if (ArtSlider[i].value >= 0)
                    degree = (int)Cylinder[i].transform.localEulerAngles.z;
                else
                    degree = (int)Cylinder[i].transform.localEulerAngles.z - 360;
                storedclass.transform_value[i] = degree;

            }
            else
            {
                if (ArtSlider[i].value >= 0)
                    degree = (int)Cylinder[i].transform.localEulerAngles.y;
                else
                    degree = (int)Cylinder[i].transform.localEulerAngles.y - 360;
                storedclass.transform_value[i] = degree;
            }
        }

        storedclass.gripperValue = GripperSlider.value;

        storedclass.Send();

        yield return new WaitForSeconds(1f);

        StartCoroutine("SendWeb");
    }

    public void onRun()
    {
        string command = "G0 A" + ArtSlider[0].value / 2 + " B" + ArtSlider[1].value / 2 + " C" + ArtSlider[1].value / 2 +
            " D" + ArtSlider[2].value / 2 + " X" + ArtSlider[3].value * -11 / 90 + " Y" + ArtSlider[4].value / 30 +
            " Z" + ArtSlider[4].value / -30 + "\n";
        Debug.Log(command);
        commController.send(command);

    }
}