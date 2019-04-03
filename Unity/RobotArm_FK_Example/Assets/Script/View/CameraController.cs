using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public float horizontalSpeed = 25f;
    public float verticalSpeed = 40f;

    public float ZoomSpeed = 20f;       // 줌 속도

    private Transform MainCamera;       // 메인 카메라 위치
    private float rotX = 0;
    private float rotY = 0;
    private Vector3 dir;

    // Use this for initialization
    void Start () {
        MainCamera = Camera.main.transform;
    }

    void Update()
    {
        Zoom();
        CameraRotation();
    }


    void Zoom()
    {
        float ZoomDistance = Input.GetAxis("Mouse ScrollWheel") * -ZoomSpeed;
        
        MainCamera.GetComponent<Camera>().fieldOfView += ZoomDistance;
    }

    void CameraRotation()
    {
        if (Input.GetMouseButton(1))
        {
            rotX = Input.GetAxis("Mouse X") * horizontalSpeed * Time.deltaTime;
            rotY = Input.GetAxis("Mouse Y") * verticalSpeed * Time.deltaTime;

            MainCamera.transform.RotateAround(transform.position, Vector3.up, -rotX);
            MainCamera.transform.RotateAround(transform.position, Vector3.right, rotY);

            //MainCamera.LookAt(transform.position);
        } 
    }

    private void LateUpdate()
    {
        MainCamera.transform.LookAt(transform.position);
    }

}
