using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{

    public List<WindowEntry> windows = new List<WindowEntry>();

    private void Start()
    {
        foreach (WindowEntry win in windows)
        {
            win.physicalWindow.SetActive(false);
            win.windowTab.SetActive(true);
        }
    }

    public void OpenWindow(string id)
    {
        WindowEntry toToggle = GrabWindowViaID(id);

        if (toToggle != null)
        {
            toToggle.physicalWindow.SetActive(true);
            toToggle.windowTab.SetActive(false);
        }
    }

    public void CloseWindow(string id)
    {
        WindowEntry toToggle = GrabWindowViaID(id);

        if (toToggle != null)
        {
            toToggle.physicalWindow.SetActive(false);
            toToggle.windowTab.SetActive(true);
        }
    }

    private WindowEntry GrabWindowViaID(string id)
    {
        foreach (WindowEntry win in windows)
        {
            if (win.winID == id)
            {
                return win;
            }
        }

        return null;
    }

}
