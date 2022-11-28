using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEnumeratorHandler : MonoBehaviour
{
    public VisualManager visMan;

    private void Start()
    {
        if (visMan != null)
        {
            StartCoroutine(GraphData());
        }
    }

    IEnumerator GraphData()
    {
        if (visMan.gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(visMan.timeBetweenGraphEntires);
            visMan.Graph();
            StartCoroutine(GraphData());
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(GraphData());
        }
    }
}
