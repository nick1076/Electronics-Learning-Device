using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO.Ports;

public class SensorText : MonoBehaviour
{
    // Soil Moisture
    public TextMeshProUGUI sensorText;
    public float moisture;

    //General
    public bool inDebug = true;
    public SelectionMenu selMen;
    //SerialPort stream = new SerialPort("COM4", 9600);

    private void Start()
    {
        //stream.Close();
        //stream.Open();
        StartCoroutine(ReadSerial());
    }

    void Update()
    {
        UpdateSensorText();
    }

    IEnumerator ReadSerial()
    {
        yield return new WaitForSeconds(.1f);

        //Data RecievalSerialPort mySerialPort = new SerialPort("COM1");
        Process();
    }

    public void Process()
    {
        if (inDebug)
        {
            Update_MoistureVisual(moisture);
            return;
        }

        SerialPort mySerialPort = new SerialPort(selMen.portSelected);
        mySerialPort.BaudRate = 9600;
        mySerialPort.Parity = Parity.None;
        mySerialPort.StopBits = StopBits.One;
        mySerialPort.DataBits = 8;
        mySerialPort.Handshake = Handshake.None;
        mySerialPort.Open();
        string value = mySerialPort.ReadLine();
        mySerialPort.Close();

        //Soil Moisture
        print(value);

        string[] values = value.Split('?');
        string[] values2 = value.Split('.');
        float soilMoisture = 0;

        if (values2.Length > 2)
        {
            StartCoroutine(ReadSerial());
            return;
        }
        if (values.Length == 1)
        {

            soilMoisture = float.Parse(value);

            if (soilMoisture > 10)
            {
                StartCoroutine(ReadSerial());
                return;
            }

            Update_MoistureVisual(soilMoisture);
            sensorText.text = soilMoisture.ToString() + "v";

            StartCoroutine(ReadSerial());
            return;
        }

        soilMoisture = float.Parse(values[1]);

        if (soilMoisture > 10)
        {
            StartCoroutine(ReadSerial());
            return;
        }

        Update_MoistureVisual(soilMoisture);
        sensorText.text = soilMoisture.ToString() + "v";

        StartCoroutine(ReadSerial());
    }

    public void UpdateSensorText()
    {
        if (inDebug)
        {
            Update_MoistureVisual(moisture);
            return;
        }
        return;

        
    }

    

    public void Update_MoistureVisual(float reading)
    {
        sensorText.text = reading.ToString() + "v";
    }

}
