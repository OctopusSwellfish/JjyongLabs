using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmController : MonoBehaviour {
    public static RobotArmController instance;
    private GameObject[] Cylinder = new GameObject[6];
    private float fSpeed = 1f;

    void Awake()
    {
        instance = this;    //

        //
        Cylinder[0] = transform.Find("Cylinder0").gameObject;
        for (int i = 0; i < Cylinder.Length - 1; i++)
            Cylinder[i + 1] = Cylinder[i].transform.Find("Cylinder" + (i + 1)).gameObject;

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        
		
	}
    public void FKMoveAll(float art1, float art2, float art3, float art4, float art5, float art6)
    {
        FKMoveArt1(art1);
        FKMoveArt2(art2);
        FKMoveArt3(art3);
        FKMoveArt4(art4);
        FKMoveArt5(art5);
        FKMoveArt6(art6);

    }

    public void FKMoveArt1(float value)
    {
        Cylinder[0].transform.localRotation = 
            Quaternion.Lerp(Cylinder[0].transform.localRotation, 
            Quaternion.Euler(0, value, 0), Time.deltaTime * fSpeed);

    }
    public void FKMoveArt2(float value)
    {
        Cylinder[1].transform.localRotation =
           Quaternion.Lerp(Cylinder[0].transform.localRotation,
           Quaternion.Euler(value, 0, 0), Time.deltaTime * fSpeed);
    }
    public void FKMoveArt3(float value)
    {
        Cylinder[2].transform.localRotation =
           Quaternion.Lerp(Cylinder[0].transform.localRotation,
           Quaternion.Euler(value, 0, 0), Time.deltaTime * fSpeed);
    }
    public void FKMoveArt4(float value)
    {
        Cylinder[3].transform.localRotation =
          Quaternion.Lerp(Cylinder[0].transform.localRotation,
          Quaternion.Euler(0, value, 0), Time.deltaTime * fSpeed);
    }
    public void FKMoveArt5(float value)
    {
        Cylinder[4].transform.localRotation =
           Quaternion.Lerp(Cylinder[0].transform.localRotation,
           Quaternion.Euler(value, 0, 0), Time.deltaTime * fSpeed);
    }
    public void FKMoveArt6(float value)
    {
        Cylinder[5].transform.localRotation =
        Quaternion.Lerp(Cylinder[0].transform.localRotation,
        Quaternion.Euler(0, value, 0), Time.deltaTime * fSpeed);
    }

    public float GetCylinderRotationX(int i)
    {
        return Cylinder[i].transform.eulerAngles.x;
    }
    public float GetCylinderRotationY(int i)
    {
        return Cylinder[i].transform.eulerAngles.y;
    }

}
