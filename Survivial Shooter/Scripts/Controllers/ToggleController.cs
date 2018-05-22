using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ToggleController : MonoBehaviour {

    private Toggle toggle;

    public Color activeColor;
    public Color normalColor;
    public Image background;

    void Awake()
    {
        toggle = GetComponent<Toggle>();

        if (QualitySettings.GetQualityLevel() == 2)
        {
            toggle.isOn = true;
        }
    }

    public void Toggle()
    {
        if(toggle.isOn)
        {
            background.color = activeColor;
        }
        else
        {
            background.color = normalColor;
        }

        ToggleVSync();
    }

    void ToggleVSync()
    {
        QualitySettings.vSyncCount = toggle.isOn ? 1 : 0; 
    }
}
