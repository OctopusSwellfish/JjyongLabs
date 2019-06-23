using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collison : MonoBehaviour {
    public static Collison instance;
    private GameObject sphere;

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("OnTriggerEnter" + " x : " + transform.position.x + " y : " + transform.position.y);
    }

    void OnTriggerExit(Collider col)
    {
        //Debug.Log("OnTriggerExit");
        this.sphere.GetComponent<MeshRenderer>().material.color = Color.green;
    }
    void OnTriggerStay(Collider col)
    {
        //Debug.Log("OOnTriggerStay");
        this.sphere.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public bool SphereCheck()
    {
        if (this.sphere.GetComponent<MeshRenderer>().material.color == Color.red)
            return true;
        else
            return false;
    }

    void Awake()
    {
        instance = this;
        this.sphere = GameObject.Find("SafeSphere");
        this.sphere.GetComponent<MeshRenderer>().material.color = Color.green;
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
