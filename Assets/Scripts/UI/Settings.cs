using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private GameObject SoundSettings, DisplaySettings, Soundsettings, Displaysettings;
    private void Start()
    {
        SoundSettings = GameObject.Find("SoundSettings");
        DisplaySettings = GameObject.Find("DisplaySettings");
        Soundsettings = GameObject.Find("Soundsettings");
        Displaysettings = GameObject.Find("Displaysettings");
        SoundSettings.SetActive(false);
        DisplaySettings.SetActive(false);
        Soundsettings.SetActive(false);
        Displaysettings.SetActive(false);
    }
    public void OpenSettings()
    {
        SoundSettings.SetActive(true);
        DisplaySettings.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
