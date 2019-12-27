using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ArmController : MonoBehaviour
{
    private GameObject[] Cylinder = new GameObject[6];
    private Slider[] ArtSlider = new Slider[6];
    private GameObject mCube;
    private Button mButton;
    private Text mText;
    private bool mStop = false;
    private bool mMoveArtCorutine = false;


    private KinectManager kinectManager;

    
    private bool isMovement = false;
    void Awake()
    {
        //
        Cylinder[0] = transform.Find("Cylinder0").gameObject;
        for (int i = 0; i < Cylinder.Length - 1; i++)
            Cylinder[i + 1] = Cylinder[i].transform.Find("Cylinder" + (i + 1)).gameObject;
        //        
        for (int i = 0; i < ArtSlider.Length; i++)
            ArtSlider[i] = GameObject.Find("Axis" + (i + 1) + "Slider").transform.GetComponent<Slider>();
        mCube = GameObject.Find("Cube");
        mButton = GameObject.Find("WorkBtn").GetComponent<Button>();

        mText = mButton.transform.Find("Text").GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine("MoveArt");
        StartCoroutine("isMovementCheckCo");
        StartCoroutine("CollsionCheck");
   

    }


    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator MoveArt()
    {
        mMoveArtCorutine = false;
        while (!mStop)
        {
            float speed = 100F;
            float degree = 0;

            for (int i = 0; i < Cylinder.Length; i++)
            {
                if (i == 0 || i == 3 || i == 5)
                {

                    if (ArtSlider[i].value >= 0)
                        degree = Cylinder[i].transform.localEulerAngles.y;
                    else
                        degree = Cylinder[i].transform.localEulerAngles.y - 360;

                    if (Math.Abs(ArtSlider[i].value - degree) > 2)
                    {
                        Cylinder[i].transform.localRotation = Quaternion.Lerp(Cylinder[i].transform.localRotation,
                            Quaternion.Euler(0, ArtSlider[i].value, 0), speed * Time.deltaTime / Quaternion.Angle(Cylinder[i].transform.localRotation,Quaternion.Euler(0, ArtSlider[i].value, 0)));
                    }
                    else
                    {
                        Cylinder[i].transform.localRotation = Quaternion.Euler(0, ArtSlider[i].value, 0);
                    }
                }
                else
                {
                    //
                    if (ArtSlider[i].value > 0)
                        degree = Cylinder[i].transform.localEulerAngles.x;
                    else
                        degree = Cylinder[i].transform.localEulerAngles.x - 360;
                    //
                    if (Math.Abs(ArtSlider[i].value - degree) > 2)
                    {
                        Cylinder[i].transform.localRotation = Quaternion.Lerp(Cylinder[i].transform.localRotation,
                            Quaternion.Euler(ArtSlider[i].value, 0, 0), speed * Time.deltaTime / Quaternion.Angle(Cylinder[i].transform.localRotation, Quaternion.Euler(0, ArtSlider[i].value, 0)));
                    }
                    else
                    {
                        Cylinder[i].transform.localRotation = Quaternion.Euler(ArtSlider[i].value, 0, 0);
                    }
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
        mMoveArtCorutine = true;
    }


    IEnumerator isMovementCheckCo()
    {
        bool result = true;
        float degree;

        while (true)
        {
            result = true;
            for (int i = 0; i < ArtSlider.Length; i++)
            {
                if (i == 0 || i == 3 || i == 5)
                {
                    if (ArtSlider[i].value >= 0)
                        degree = Cylinder[i].transform.localEulerAngles.y;
                    else
                        degree = Cylinder[i].transform.localEulerAngles.y - 360;

                    if ((int)ArtSlider[i].value == Convert.ToInt32(degree))
                        result = result && true;
                    else
                        result = result && false;
                }
                else
                {
                    if (ArtSlider[i].value >= 0)
                        degree = Cylinder[i].transform.localEulerAngles.x;
                    else
                        degree = Cylinder[i].transform.localEulerAngles.x - 360;

                    if ((int)ArtSlider[i].value == Convert.ToInt32(degree))
                        result = result && true;
                    else
                        result = result && false;
                }
            }

            isMovement = result;
            yield return new WaitForSeconds(0.1f);

        }
    }

    public void TransformZeroPosition()
    {
        for (int i = 0; i < ArtSlider.Length; i++)
        {
            ArtSlider[i].value = 0;
        }
    }

    public void OnClickTest()
    {

        StartCoroutine("Test1");

    }
    IEnumerator Test1()
    {

        EnableSlider(false);
        mCube.transform.position = new Vector3(-7, 0.75f, -7);
        TransformValue(45, -65, -50, 0, -65, -45);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitWhile(() => isMovement != true);
        mCube.transform.parent = Cylinder[5].transform;
        TransformValue(45, -35, -45, 0, -65, -45);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitWhile(() => isMovement != true);
        TransformValue(-45, -35, -45, 0, -65, -45);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitWhile(() => isMovement != true);
        TransformValue(-135, -35, -45, 0, -65, -45);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitWhile(() => isMovement != true);
        TransformValue(-135, -65, -50, 0, -65, -45);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitWhile(() => isMovement != true);
        mCube.transform.parent = null;
        TransformValue(-135, -35, -45, 0, -65, -45);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitWhile(() => isMovement != true);
        TransformValue(0, 0, 0, 0, 0, 0);
        yield return new WaitWhile(() => isMovement != true);
        EnableSlider(true);
    }

    public void EnableSlider(bool enabled)
    {
        for (int i = 0; i < ArtSlider.Length; i++)
            ArtSlider[i].enabled = enabled;

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

    public void StopArt()
    {
        if (mText.text.Equals("Stop"))
        {
            mText.text = "Start";
            mStop = true;
        }
        else
        {
            mText.text = "Stop";
            mStop = false;
            StartCoroutine("MoveArt");
        }

    }

    IEnumerator CollsionCheck()
    {
        while (true)
        {
            if (Collison.instance.SphereCheck())
            {
                mText.text = "Start";
                mStop = true;
            }
            else
            {
                mText.text = "Stop";
                mStop = false;
                if(mMoveArtCorutine)
                    StartCoroutine("MoveArt");
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

}
