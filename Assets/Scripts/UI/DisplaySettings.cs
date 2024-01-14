using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySettings : MonoBehaviour
{
    private GameObject Displaysettings, displaySettings, Soundsettings;
    void Start()
    {
        Displaysettings = GameObject.Find("Displaysettings");
        Soundsettings = GameObject.Find("Soundsettings");
        displaySettings = this.gameObject;
    }
    public void OpenDisplaySettings()
    {
        Soundsettings.SetActive(false);
        Displaysettings.SetActive(true);
    }
}
