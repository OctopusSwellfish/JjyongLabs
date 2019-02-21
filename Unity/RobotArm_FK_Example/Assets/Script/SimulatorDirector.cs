using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SerialComm;
using System;

public class SimulatorDirector : MonoBehaviour {

    private Slider[] _axisSlider = new Slider[6];   //

    private UsbSerial _usbSerial;
    private GrblCommand _grblCommand;
    

    void Awake() {
        _usbSerial = new UsbSerial();
        _usbSerial.Open();

        for (int i = 0; i < _axisSlider.Length; i++)
            _axisSlider[i] = GameObject.Find("Axis" + (i + 1)+ "Slider").transform.GetComponent<Slider>();



    }

	// Use this for initialization
	void Start () {

        //thorContorller.Setting(_axisSlider);
        

    }

    // Update is called once per frame
    void Update () {
        if(Math.Abs(RobotArmController.instance.GetCylinderRotationY(0) - _axisSlider[0].value) > 2)
            RobotArmController.instance.FKMoveArt1(_axisSlider[0].value);
        if (Math.Abs(RobotArmController.instance.GetCylinderRotationY(1) - _axisSlider[0].value) > 2)
            RobotArmController.instance.FKMoveArt1(_axisSlider[0].value);
        RobotArmController.instance.FKMoveArt2(_axisSlider[1].value);
        RobotArmController.instance.FKMoveArt3(_axisSlider[2].value);
        RobotArmController.instance.FKMoveArt4(_axisSlider[3].value);
        RobotArmController.instance.FKMoveArt5(_axisSlider[4].value);
        RobotArmController.instance.FKMoveArt6(_axisSlider[5].value);
    }

    void OnApplicationQuit()
    {
        _usbSerial.Close();
    }
}
