using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GripperController : MonoBehaviour
{


    public GameObject idlerGearR, pivotArmR, idlerGearAR, GripperR; // Right Gripper
    public GameObject ServoGearL, pivotArmL, ServoGearAL, GripperL; // Left Gripper
    public GameObject GripperColliderR, GripperColliderL; // Gripper Collider

    private bool check = false;
    private int maxValue = 85;
    private Slider mGripperSlider;
    private GripperCollision GripperCollisionR, GripperCollisionL;


    void Awake()
    {
        mGripperSlider = GameObject.Find("GripperSlider").transform.GetComponent<Slider>();
        GripperCollisionR = GripperColliderR.GetComponent<GripperCollision>();
        GripperCollisionL = GripperColliderL.GetComponent<GripperCollision>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject other = GripperCollisionR.getOtherCollider();
        if (other != null )
        {
            if (GripperCollisionR.getIsCollision() && GripperCollisionL.getIsCollision())
            {
                other.transform.parent = transform;
                other.GetComponent<Rigidbody>().useGravity = false;
                if (!check)
                {
                    maxValue = (int)mGripperSlider.value;
                    check = true;
                }
            }
            else if (!GripperCollisionR.getIsCollision() && !GripperCollisionL.getIsCollision())
            {
                other.transform.parent = null;
                other.GetComponent<Rigidbody>().useGravity = true;

                other.transform.rotation = Quaternion.Euler(0, 0, 0);
                GripperCollisionR.setOtherCollider(null);
                maxValue = 85;
                check = false;
            }
        }

        if((int)mGripperSlider.value >= maxValue)
        {
            mGripperSlider.value = maxValue;
        }
    }

    void FixedUpdate()
    {
        // Move Right Gripper 
        idlerGearR.transform.localRotation = Quaternion.Euler(0, 0, mGripperSlider.value);
        pivotArmR.transform.localRotation = Quaternion.Euler(0, 0, mGripperSlider.value);
        GripperR.transform.position = idlerGearAR.transform.position;

        // Move Left Gripper 
        ServoGearL.transform.localRotation = Quaternion.Euler(0, 0, -mGripperSlider.value);
        pivotArmL.transform.localRotation = Quaternion.Euler(0, 0, -mGripperSlider.value);
        GripperL.transform.position = ServoGearAL.transform.position;
    }
}
