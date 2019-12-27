using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collison : MonoBehaviour {
    public static Collison instance;
    private GameObject TestSphere;
    private GameObject sphere;

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("OnTriggerEnter" + " x : " + transform.position.x + " y : " + transform.position.y);
 
        RaycastHit hit;
        if (Physics.Raycast(transform.position, col.transform.position , out hit))
        {
            TestSphere.transform.position = hit.point;
        }

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
        this.TestSphere = GameObject.Find("TestSphere");
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
