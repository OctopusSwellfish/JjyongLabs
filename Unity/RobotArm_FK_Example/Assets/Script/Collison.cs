using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collison : MonoBehaviour {
    public static Collison instance;
    private GameObject Plane;

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("OnTriggerEnter" + " x : " + transform.position.x + " y : " + transform.position.y);
    }

    void OnTriggerExit(Collider col)
    {
        //Debug.Log("OnTriggerExit");
        this.Plane.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }
    void OnTriggerStay(Collider col)
    {
        //Debug.Log("OOnTriggerStay");
        this.Plane.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public bool PlaneCheck()
    {
        if (this.Plane.GetComponent<MeshRenderer>().material.color == Color.red)
            return true;
        else
            return false;
    }

    void Awake()
    {
        instance = this;
        this.Plane = GameObject.Find("Plane");
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
