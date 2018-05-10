using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TechTweaking.Bluetooth;

public class BluetoothAndroidWrapper
{

    private BluetoothDevice device;
    private static BluetoothAndroidWrapper instance;
    private const string HW_ID = "VIBE1";
   
    public bool isConnected
    {
        get
        {
            return device.IsConnected;
        }
    }

    public void SetConnectedHandlar(Action<BluetoothDevice> handle)
    {
        device.OnConnected += handle;
    }
    public void ResetConnectedHandlar(Action<BluetoothDevice> handle)
    {
        device.OnConnected -= handle;
    }

    public void SetDisconnectedHandlar(Action<BluetoothDevice> handle)
    {
        device.OnDisconnected += handle;
    }
    public void ReSetDisconnectedHandlar(Action<BluetoothDevice> handle)
    {
        device.OnDisconnected -= handle;
    }

    private BluetoothAndroidWrapper()
    {
        device = new BluetoothDevice();
        device.Name = HW_ID;
    }
    public static BluetoothAndroidWrapper getInstance()
    {
        if (instance == null)
        {
            BluetoothAdapter.enableBluetooth();
            instance = new BluetoothAndroidWrapper();            
        }
        return instance;
    }
    
    public void connect()
    {
        if( !device.IsConnected )
        {
            Debug.Log("Device Connect..... " + HW_ID);
            device.connect();
            
        }
    }
    public void disconnect()
    {
        if( device!= null )
        {
            device.close();
        }
    }
    public void Send(char sequance)
    {
        if( device != null && device.IsConnected )
        {
            char[] sendData = new char[] { sequance };
            device.send(System.Text.Encoding.ASCII.GetBytes(sendData));
        }
    }

   
}