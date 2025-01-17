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

    //Window - Electron Visual
    [Header("Window - Electron Visual")]
    public SpriteRenderer electronChannel;
    public SpriteRenderer electronChannelBorder;
    public ParticleSystem electrons;

    //Window - Source Visual
    [Header("Window - Source Visual")]
    public GameObject AA;
    public GameObject AAx3;
    public GameObject nineVolt;
    public GameObject missing;
    public GameObject unknown;

    //Window - Graph
    [Header("Window - Graph")]
    public VoltageGraph vGraph;

    //Window - High/Low Visual
    [Header("Window - High/Low Visual")]
    public GameObject high;
    public GameObject low;

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
        if (id == 0)
        {
            missing.gameObject.SetActive(true);
            nineVolt.gameObject.SetActive(false);
            AA.gameObject.SetActive(false);
            unknown.gameObject.SetActive(false);
            AAx3.gameObject.SetActive(false);
        }
        else if (id == 1)
        {
            missing.gameObject.SetActive(false);
            nineVolt.gameObject.SetActive(false);
            AA.gameObject.SetActive(true);
            unknown.gameObject.SetActive(false);
            AAx3.gameObject.SetActive(false);
        }
        else if (id == 2)
        {
            missing.gameObject.SetActive(false);
            nineVolt.gameObject.SetActive(true);
            AA.gameObject.SetActive(false);
            unknown.gameObject.SetActive(false);
            AAx3.gameObject.SetActive(false);
        }
        else if (id == 3)
        {
            missing.gameObject.SetActive(false);
            nineVolt.gameObject.SetActive(false);
            AA.gameObject.SetActive(false);
            unknown.gameObject.SetActive(true);
            AAx3.gameObject.SetActive(false);
        }
        else if (id == 4)
        {
            missing.gameObject.SetActive(false);
            nineVolt.gameObject.SetActive(false);
            AA.gameObject.SetActive(false);
            unknown.gameObject.SetActive(false);
            AAx3.gameObject.SetActive(true);
        }
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
        electronChannel.transform.localScale = new Vector3(electronChannel.transform.localScale.x, wireGauge, electronChannel.transform.localScale.z);
        electronChannelBorder.transform.localScale = new Vector3(electronChannelBorder.transform.localScale.x, wireGauge + 0.45f, electronChannelBorder.transform.localScale.z);

        if (reading <= 0)
        {
            electrons.Stop();
        }
        else
        {
            electrons.Play();
            var sh = electrons.shape;
            var sp = electrons.main;
            sh.radius = wireGauge / 6.1f;
            sp.startSpeed = reading * 2.5f;
        }

        if (reading <= 0)
        {
            low.SetActive(true);
            high.SetActive(false);
        }
        else
        {
            low.SetActive(false);
            high.SetActive(true);
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
