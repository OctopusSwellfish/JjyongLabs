using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotArmController : MonoBehaviour {

    GameObject collisionDetectionText;

    void OnCollisonEnter(Collision other)
    {
        Debug.Log(" Detection.");
        this.collisionDetectionText.GetComponent<Text>().text = " Detection.";
    }


    // Use this for initialization
    void Start()
    {

        this.collisionDetectionText = GameObject.Find("CollisionDetectionText");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
