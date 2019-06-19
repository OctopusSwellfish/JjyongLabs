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
    void Start()
    {

    }

   public void Send()
    {
        string json_send = JsonUtility.ToJson(this);
        UnityWebRequest www = UnityWebRequest.Put("http://127.0.0.1:3000/DB/", json_send);
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

    public GameObject[] Cylinder = new GameObject[6];
    public Slider[] ArtSlider = new Slider[6];
    public Slider GripperSlider;

    public GameObject gripper;


    public Slider KinematicsSlider;
    public static double[] theta = new double[6];    //angle of the joints

    private bool FKinematics = true;
    private bool isMovement = true;
    private GameObject mCube;
    private bool gripperCheck = false;

    private bool mStop = false;
    private bool mMoveArtCorutine = false;
    private bool mUndoTest1 = false;

    private int step = 0;

    private GripperController gripperController;

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
                Cylinder[0].transform.localEulerAngles = new Vector3(0, 0, (    float)theta[0] * Mathf.Rad2Deg);

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

        StartCoroutine("Test1");
        Debug.Log("온클릭 테스트");

    }

    IEnumerator GrippedObject()
    {
        if (gripperController.getMaxValue() > GripperSlider.value + 1)
        {
            Debug.Log("" + gripperController.getMaxValue());
            GripperSlider.value++;
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
            GripperSlider.value--;
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
        yield return new WaitWhile(() => isMovement != true);
        // Step 1
        if (step <= 0)
        {
            TransformValue(0, 60, 75, 0, 0, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
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
            TransformValue(0, 25, 60, 0, 0, 0);
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => isMovement != true);
            step = 3;
        }
        // Step 4
        if (step <= 3)
        {
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
    }

    IEnumerator unTest1()
    {

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
        }
        StartCoroutine("Test1");


    }


    IEnumerator CollsionCheck()
    {

        if (Collison.instance.SphereCheck())
        {
            mStop = true;

            storedclass.result = "true";
          //  storedclass.Send();

            StopCoroutine("Test1");
            StopCoroutine("GrippedObject");
            StopCoroutine("UngrippedObject");
            if (!mUndoTest1)
            {
                test2();
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
        storedclass.transform_value[0] = Cylinder[0].transform.localEulerAngles.z;
        storedclass.transform_value[1] = Cylinder[1].transform.localEulerAngles.y;
        storedclass.transform_value[2] = Cylinder[2].transform.localEulerAngles.y;
        storedclass.transform_value[3] = Cylinder[3].transform.localEulerAngles.z;
        storedclass.transform_value[4] = Cylinder[4].transform.localEulerAngles.y;
        storedclass.transform_value[5] = Cylinder[5].transform.localEulerAngles.z;

        storedclass.Send();

        yield return new WaitForSeconds(0.1f);

        StartCoroutine("SendWeb");
    }
}