using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using SerialComm;

public class CommController : MonoBehaviour
{
    public static CommController instance;

    private SerialPort _SerialPort = new SerialPort();
    private UsbSerial usbSerial = new UsbSerial();
    private bool fristRecv = false;

    public bool FristRecv
    {
        get
        {
            return fristRecv;
        }

        set
        {
            fristRecv = value;
        }
    }

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        usbSerial.Open();
        StartCoroutine(recv());
    }

    // Update is called once per frame
    void Update()
    {
    }

    
   
    IEnumerator recv()
    {
        if (usbSerial.IsOpen)
        {
            string temp = usbSerial.ReadLineBlocking();
            //Debug.Log(temp);
            if (temp != null)
            {
                Debug.Log(temp);
                fristRecv = true;
                yield return new WaitForSeconds(0.1f);
                Debug.Log(usbSerial.ReadLineBlocking());
            }
        }
        yield return new WaitForSeconds(0.5f);
        if (fristRecv == false)
        {
            StartCoroutine(recv());
        }
    }


    public void send(string text)
    {
        if(fristRecv != false)
        {
            usbSerial.Write(text);
            fristRecv = false;
            StartCoroutine(recv());
        }
    }


    void OnApplicationQuit()
    {
        usbSerial.Close();
    }
}
