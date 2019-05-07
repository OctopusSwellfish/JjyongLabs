using UnityEngine;
using System.IO.Ports;
using System.IO;
using System;

namespace SerialComm
{
    class UsbSerial
    {
        private SerialPort _serialPort = new SerialPort();

        private string mPortName;
        private int mBaudRate;

        public UsbSerial()
        {
            mPortName = "COM4"; 
            mBaudRate = (int)115200;  
        }
        public UsbSerial(string strPortName, int nBaudRate)
        {
            mPortName = strPortName;
            mBaudRate = nBaudRate;
        }

        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.PortName = mPortName;   // 
                    _serialPort.BaudRate = mBaudRate;   // 

                    _serialPort.DataBits = 8;           //
                    _serialPort.Parity = Parity.None;   //
                    _serialPort.StopBits = StopBits.One;//
                    _serialPort.ReadTimeout = 50;       //
                    _serialPort.WriteTimeout = 1;       //
                    _serialPort.NewLine = "\n";         //

                    _serialPort.Open();
                    _serialPort.DiscardOutBuffer();
                    _serialPort.DiscardInBuffer();
                }
                catch (IOException e) { Debug.Log(e); return; }
                catch (Exception e) { Debug.Log(e); return; }
            }
        }


        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                try { _serialPort.DiscardOutBuffer(); }
                catch (Exception e) { Debug.Log(e); }
                try { _serialPort.DiscardInBuffer(); }
                catch (Exception e) { Debug.Log(e); }
                try { _serialPort.Close(); }
                catch (Exception e) { Debug.Log(e); }
            }
        }

        public bool IsOpen
        { get { return _serialPort.IsOpen; } }


        public void Write(byte b)
        {
            _serialPort.Write(new byte[] { b }, 0, 1);
        }

        public void Write(string text)
        {
            _serialPort.Write(text);
        }

        public string ReadLineBlocking()
        {
            try { return _serialPort.ReadLine(); }
            catch (TimeoutException e) { Debug.Log(e); return null; }
            catch (Exception e) { Debug.Log(e); return null; }

        }
    }
}