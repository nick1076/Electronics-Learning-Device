using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO.Ports;
using UnityEngine.UI;

public class SelectionMenu : MonoBehaviour
{

    public TMP_Dropdown comSelection;
    public GameObject primaryMenu;
    public GameObject serialSelectMenu;
    public Button continueButton;

    public string debugPortName = "Debug Port";
    public string portSelected = "NULL";
    private List<string> ports = new List<string>();

    private void Start()
    {
        string[] portsList = SerialPort.GetPortNames();

        ports.Add(debugPortName);

        foreach (string p in portsList)
        {
            ports.Add(p);
        }

        TMP_Dropdown.OptionDataList list = new TMP_Dropdown.OptionDataList();

        foreach (string port in ports)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = port;
            list.options.Add(data);
        }

        comSelection.AddOptions(list.options);
    }

    private void Update()
    {
        if (comSelection.value != 0)
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
    }

    public void Continue()
    {
        portSelected = ports[comSelection.value - 1];
        if (portSelected == debugPortName)
        {
            primaryMenu.GetComponent<VisualManager>().inDebug = true;
        }
        else
        {
            primaryMenu.GetComponent<VisualManager>().inDebug = false;
        }

        primaryMenu.SetActive(true);
        serialSelectMenu.SetActive(false);
    }

}
