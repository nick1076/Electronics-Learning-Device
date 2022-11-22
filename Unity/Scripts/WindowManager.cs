using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WindowManager : MonoBehaviour
{

    public List<WindowEntry> windows = new List<WindowEntry>();

    //Modes
    public List<ModeData> possibleModes = new List<ModeData>();
    public TMP_Dropdown modeSelector;

    private void Start()
    {
        if (modeSelector != null)
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            foreach (ModeData mode in possibleModes)
            {
                TMP_Dropdown.OptionData cOption = new TMP_Dropdown.OptionData();
                cOption.text = mode.modeName;
                options.Add(cOption);
            }

            modeSelector.AddOptions(options);
        }
        foreach (WindowEntry win in windows)
        {
            win.physicalWindow.SetActive(false);
            win.windowTab.SetActive(true);
        }
        if (modeSelector != null)
        {
            SelectMode(GetModeByName(modeSelector.options[modeSelector.value].text));
        }
    }

    private ModeData GetModeByName(string name)
    {
        foreach (ModeData mode in possibleModes)
        {
            if (mode.modeName == name)
            {
                return mode;
            }
        }

        return null;
    }

    public void OnModeDropdownSelect()
    {
        SelectMode(GetModeByName(modeSelector.options[modeSelector.value].text));
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

    public void SelectMode(ModeData mode)
    {
        for (int i = 0; i < windows.Count; i++)
        {
            if (!mode.permittedWindows.Contains(windows[i].winID))
            {
                windows[i].physicalWindow.SetActive(false);
                windows[i].windowTab.SetActive(false);
            }
            else
            {
                if (!windows[i].physicalWindow.activeInHierarchy)
                {
                    windows[i].windowTab.SetActive(true);
                }
            }
        }
    }

}
