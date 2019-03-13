using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GripperController : MonoBehaviour
{

    private Slider mGripperSlider;

    public GameObject IddleGearR, PivorArmR, IddleGearAR, GripperR; // Right Gripper
    public GameObject ServoGearL, PivorArmL, ServoGearAL, GripperL; // Left Gripper

    public GameObject GripperColliderR, GripperColliderL; // Gripper Collider

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
        if (GripperCollisionR.getIsCollision() && GripperCollisionL.getIsCollision())
        {
            other.transform.parent = transform;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<Rigidbody>().useGravity = false;
        }
        else if (!GripperCollisionR.getIsCollision() && !GripperCollisionL.getIsCollision())
        {
            other.transform.parent = null;
            other.GetComponent<Rigidbody>().isKinematic = false;
            other.GetComponent<Rigidbody>().useGravity = true;
        }
        
    }

    void FixedUpdate()
    {
        // Move Right Gripper 
        IddleGearR.transform.localRotation = Quaternion.Euler(0, 0, mGripperSlider.value);
        PivorArmR.transform.localRotation = Quaternion.Euler(0, 0, mGripperSlider.value);
        GripperR.transform.position = IddleGearAR.transform.position;

        // Move Left Gripper 
        ServoGearL.transform.localRotation = Quaternion.Euler(0, 0, -mGripperSlider.value);
        PivorArmL.transform.localRotation = Quaternion.Euler(0, 0, -mGripperSlider.value);
        GripperL.transform.position = ServoGearAL.transform.position;
    }
}
