using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripperCollision: MonoBehaviour {

    private GameObject otherCollider = null;
    private bool isCollision = false;

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("OnCollisionEnter");
        isCollision = true;
        otherCollider = col.gameObject;;
    }
    void OnCollisionExit(Collision col)
    {
        isCollision = false;
        Debug.Log("OnCollisionExit");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool getIsCollision()
    {
        return isCollision;
    }
    public GameObject getOtherCollider()
    {
        return otherCollider;
    }
}
