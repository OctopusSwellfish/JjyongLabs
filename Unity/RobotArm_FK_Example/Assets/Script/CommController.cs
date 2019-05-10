using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class CommController : MonoBehaviour
{
    public static CommController instance;
    private SerialPort _SerialPort = new SerialPort();

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        _SerialPort.PortName = "COM4";
        _SerialPort.BaudRate = (int)115200;

        _SerialPort.DataBits = 8;
        _SerialPort.Parity = Parity.None;
        _SerialPort.StopBits = StopBits.One;
        _SerialPort.ReadTimeout = 50;
        _SerialPort.WriteTimeout = 1;
        _SerialPort.NewLine = "\n";

        _SerialPort.Open();
        _SerialPort.DiscardOutBuffer();
        _SerialPort.DiscardInBuffer();
    }

    // Update is called once per frame
    void Update()
    {
        if (_SerialPort.IsOpen)
        {
            Debug.Log(ReadLineBlocing());
        }
    }

    public string ReadLineBlocing()
    {
        try{ return _SerialPort.ReadLine(); } catch { return null; }

    }

    public void Write(byte b)
    {
        _SerialPort.Write(new byte[] { b }, 0, 1);
    }

    public void Write(string text)
    {
        _SerialPort.Write(text);
    }

    void OnApplicationQuit()
    {
        if (_SerialPort.IsOpen)
        {
            try { _SerialPort.DiscardOutBuffer(); } catch { }
            try { _SerialPort.DiscardInBuffer(); } catch { }
            try { _SerialPort.Close(); } catch { }
        }
    }
}
