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

    //Electron Visual
    public SpriteRenderer electronChannel;
    public SpriteRenderer electronChannelBorder;
    public ParticleSystem electrons;

    //Source Visual
    public GameObject AA;
    public GameObject nineVolt;
    public GameObject missing;
    public GameObject unknown;

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

    public void DisplaySource(int id)
    {
        if (id == 0)
        {
            missing.gameObject.SetActive(true);
            nineVolt.gameObject.SetActive(false);
            AA.gameObject.SetActive(false);
            unknown.gameObject.SetActive(false);
        }
        else if (id == 1)
        {
            missing.gameObject.SetActive(false);
            nineVolt.gameObject.SetActive(false);
            AA.gameObject.SetActive(true);
            unknown.gameObject.SetActive(false);
        }
        else if (id == 2)
        {
            missing.gameObject.SetActive(false);
            nineVolt.gameObject.SetActive(true);
            AA.gameObject.SetActive(false);
            unknown.gameObject.SetActive(false);
        }
        else if (id == 3)
        {
            missing.gameObject.SetActive(false);
            nineVolt.gameObject.SetActive(false);
            AA.gameObject.SetActive(false);
            unknown.gameObject.SetActive(true);
        }
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
        electronChannel.transform.localScale = new Vector3(electronChannel.transform.localScale.x, reading, electronChannel.transform.localScale.z);
        electronChannelBorder.transform.localScale = new Vector3(electronChannelBorder.transform.localScale.x, reading + 0.45f, electronChannelBorder.transform.localScale.z);

        if (reading <= 0)
        {
            electrons.Stop();
        }
        else
        {
            electrons.Play();
            var sh = electrons.shape;
            sh.radius = reading / 6.1f;
        }

        if (reading <= 0)
        {
            DisplaySource(0);
        }
        else if (reading > 1 && reading < 2)
        {
            DisplaySource(1);
        }
        else if (reading > 8 && reading < 11)
        {
            DisplaySource(2);
        }
        else
        {
            DisplaySource(3);
        }
    }

}
