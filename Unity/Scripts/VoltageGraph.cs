using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VoltageGraph : MonoBehaviour
{
    public List<GraphEntry> points = new List<GraphEntry>();
    public GameObject pointParent;
    public List<float> voltages = new List<float>();

    public TextMeshProUGUI minV;
    public TextMeshProUGUI maxV;
    public TextMeshProUGUI midV;
    public float totalRange;

    public float maxPointPos = 99;
    public float minPointPos = 15.1f;

    public float maxSeenVoltage;
    public float minSeenVoltage;

    public List<LineRenderer> connectors = new List<LineRenderer>();

    private void Start()
    {
        totalRange = maxPointPos - minPointPos;
        foreach(Transform child in pointParent.GetComponentInChildren<Transform>())
        {
            points.Add(child.gameObject.GetComponent<GraphEntry>());
        }
    }

    public void AddVoltageEntry(float v)
    {
        if (v < 0)
        {
            v = Mathf.Abs(v);
            Debug.Log("Voltage was negative");
        }
        maxSeenVoltage = 0;
        minSeenVoltage = 0;

        for (int i = voltages.Count - 1; i > voltages.Count - 1 - points.Count; i--)
        {
            if (voltages[i] > maxSeenVoltage)
            {
                maxSeenVoltage = voltages[i];
            }
        }
        minSeenVoltage = maxSeenVoltage;
        for (int i = voltages.Count - 1; i > voltages.Count - 1 - points.Count; i--)
        {
            if (voltages[i] < minSeenVoltage)
            {
                minSeenVoltage = voltages[i];
            }
        }

        maxV.text = maxSeenVoltage.ToString() + "v";
        midV.text = ((Mathf.Round((((maxSeenVoltage - minSeenVoltage) / 2) + minSeenVoltage) * 100)) / 100).ToString() + "v";
        minV.text = minSeenVoltage.ToString() + "v";

        voltages.Add(v);
        int pointPos = points.Count - 1;
        bool centralize = false;

        if (maxSeenVoltage == minSeenVoltage)
        {
            centralize = true;
        }

        
        for (int i = voltages.Count - 1; i > voltages.Count - 1 - points.Count; i--)
        {
            if (!centralize)
            {
                GraphEntry cur = points[pointPos];
                cur.val.text = i.ToString();
                cur.point.GetComponent<RectTransform>().anchoredPosition = new Vector2(cur.point.GetComponent<RectTransform>().anchoredPosition.x, ((voltages[i] - minSeenVoltage) / (maxSeenVoltage - minSeenVoltage)) * totalRange + minPointPos);
            }
            else
            {
                GraphEntry cur = points[pointPos];
                cur.val.text = i.ToString();
                cur.point.GetComponent<RectTransform>().anchoredPosition = new Vector2(cur.point.GetComponent<RectTransform>().anchoredPosition.x, (totalRange / 2) + minPointPos);
            }

            pointPos--;

            if (pointPos == -1)
            {
                break;
            }
        } 

        for (int i = 0; i < connectors.Count; i++)
        {
            connectors[i].SetPosition(0, points[i].point.transform.position);
            connectors[i].SetPosition(1, points[i + 1].point.transform.position);
        }
    }
}
