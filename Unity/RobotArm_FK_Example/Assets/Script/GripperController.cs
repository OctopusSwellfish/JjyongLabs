using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GripperController : MonoBehaviour {

    private Slider mGripperSlider;
    
    public GameObject IddleGearR, PivorArmR, IddleGearAR, GripperR; // Right Gripper
    public GameObject ServoGearL, PivorArmL, ServoGearAL, GripperL; // Left Gripper



    void Awake()
    {
        mGripperSlider = GameObject.Find("GripperSlider").transform.GetComponent<Slider>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
