using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowObject : MonoBehaviour
{
    public bool on = false;
    public bool dragging = false;
    public Canvas canv;

    public void BeginMoving()
    {
        dragging = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            dragging = false;
        }

        if (dragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            this.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }


}
