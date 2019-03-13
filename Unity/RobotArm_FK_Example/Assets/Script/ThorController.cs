using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThorController : MonoBehaviour {

    public GameObject[] Cylinder = new GameObject[6];
    public Slider[] ArtSlider = new Slider[6];

    private bool mStop = false;

    // Use this for initialization
    void Start () {
        StartCoroutine("MoveArt");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator MoveArt()
    {
        while (!mStop)
        {
            float speed = 100F;
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
                            Quaternion.Euler(0, 0, ArtSlider[i].value), speed * Time.deltaTime / Quaternion.Angle(Cylinder[i].transform.localRotation, Quaternion.Euler(0, 0 , ArtSlider[i].value)));
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
                            Quaternion.Euler( 0, ArtSlider[i].value, 0), speed * Time.deltaTime / Quaternion.Angle(Cylinder[i].transform.localRotation, Quaternion.Euler(0, ArtSlider[i].value, 0)));
                    }
                    else
                    {
                        Cylinder[i].transform.localRotation = Quaternion.Euler(0 , ArtSlider[i].value, 0);
                    }
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

}
