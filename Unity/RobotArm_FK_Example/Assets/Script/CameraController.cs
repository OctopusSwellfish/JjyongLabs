using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float RotationSpeed = 10f;  // 회전 속도
    public float ZoomSpeed = 20f;       // 줌 속도

    private Transform InitPos;          // 카메라 초기 위치
    private Transform MainCamera;       // 메인 카메라 위치

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
            float rotX = Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime;
            float rotY = Input.GetAxis("Mouse Y") * RotationSpeed * Time.deltaTime;

            MainCamera.transform.Rotate(Vector3.up, -rotX);
            MainCamera.transform.Rotate(Vector3.right, rotY);
        }
    }
}
