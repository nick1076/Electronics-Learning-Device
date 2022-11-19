using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalButton : MonoBehaviour
{

    public UnityEvent onClick;
    public bool exitMouseOnEvent = true;

    bool on;

    private void OnMouseEnter()
    {
        on = true;
    }

    private void OnMouseExit()
    {
        on = false;
    }

    private void Update()
    {
        if (on && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (exitMouseOnEvent)
            {
                on = false;
            }
            
            onClick.Invoke();
        }
    }
}
