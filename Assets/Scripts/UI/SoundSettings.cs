using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettings : MonoBehaviour
{
    private GameObject Soundsettings, soundSettings, Displaysettings;
    void Start()
    {
        Soundsettings = GameObject.Find("Soundsettings");
        Displaysettings = GameObject.Find("Displaysettings");
        soundSettings = this.gameObject;
    }
    public void OpenSoundSettings()
    {
        Displaysettings.SetActive(false);
        Soundsettings.SetActive(true);
    }
}
