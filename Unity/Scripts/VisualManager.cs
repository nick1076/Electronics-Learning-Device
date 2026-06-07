using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO.Ports;

public class VisualManager : MonoBehaviour
{
    //General
    [Header("General")]
    public bool inDebug = true;
    public float timeBetweenGraphEntires = 0.1f;
    public float timeBetweenVoltageReadings = 0.1f;

    // Com Select Menu
    [Header("Com Select Menu")]
    public SelectionMenu selMen;

    // Window - Voltage Value
    [Header("Window - Voltage Value")]
    public TextMeshProUGUI sensorText;
    public float voltage;

    //Window - Graph
    [Header("Window - Graph")]
    public VoltageGraph vGraph;

    //Window - Debug Menu
    [Header("Window - Debug Menu")]
    public Slider voltageOverideSlider;
    public TextMeshProUGUI voltageOverideText;
    public Slider graphingTimeSlider;
    public TextMeshProUGUI graphingTimeText;
    public float graphingTimeRange = 2;
    public TextMeshProUGUI comName;

    //Hidden
    SerialPort mySerialPort;

    private void Start()
    {
        comName.text = "Selected Port: " + selMen.portSelected;

        if (selMen.portSelected == "NULL")
        {
            inDebug = true;
        }

        if (!inDebug)
        {
            //Disable Specific Debug Tools
            voltageOverideSlider.enabled = false;
            voltageOverideText.text = "";

            mySerialPort = new SerialPort(selMen.portSelected);
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
        }

        StartCoroutine(ReadSerial());
    }

    private void Update()
    {
        graphingTimeSlider.minValue = timeBetweenVoltageReadings;
        graphingTimeSlider.maxValue = Mathf.RoundToInt(timeBetweenVoltageReadings + graphingTimeRange);

        if (graphingTimeSlider.value < graphingTimeSlider.minValue)
        {
            graphingTimeSlider.value = graphingTimeSlider.minValue;
        }

        if (graphingTimeSlider.value > graphingTimeSlider.maxValue)
        {
            graphingTimeSlider.value = graphingTimeSlider.maxValue;
        }

        timeBetweenGraphEntires = (Mathf.Round(graphingTimeSlider.value * 10)) / 10;

        if (timeBetweenGraphEntires == 1)
        {
            graphingTimeText.text = timeBetweenGraphEntires.ToString() + " Second";
        }
        else
        {
            graphingTimeText.text = timeBetweenGraphEntires.ToString() + " Seconds";
        }
    }

    public void Graph()
    {
        vGraph.AddVoltageEntry(voltage);
        print("Logging voltage");
    }

    public void DisplaySource(int id)
    {

    }

    IEnumerator ReadSerial()
    {
        yield return new WaitForSeconds(timeBetweenVoltageReadings);
        Process();
    }

    public void Process()
    {
        if (inDebug)
        {
            float vNew = voltageOverideSlider.value;
            vNew = (Mathf.Round(vNew * 10)) / 10;
            voltage = vNew;
            voltageOverideText.text = vNew.ToString() + "v";
            UpdateVoltageVisuals(voltage);
            StartCoroutine(ReadSerial());
            return;
        }

        if (mySerialPort.IsOpen == false)
        {
            mySerialPort.Open();
        }
        string value = mySerialPort.ReadLine();
        mySerialPort.BaseStream.Flush();

        //Voltage
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

            UpdateVoltageVisuals(soilMoisture);
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

        UpdateVoltageVisuals(soilMoisture);
        sensorText.text = soilMoisture.ToString() + "v";

        StartCoroutine(ReadSerial());
    }

    public void UpdateVoltageVisuals(float reading)
    {
        voltage = reading;
        sensorText.text = reading.ToString() + "v";

        float wireGauge = 4;

        if (reading <= 0)
        {

        }
        else
        {

        }

        if (reading <= 0)
        {

        }
        else
        {

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
        else if (reading > 4 && reading < 5)
        {
            DisplaySource(4);
        }
        else
        {
            DisplaySource(3);
        }
    }

}
