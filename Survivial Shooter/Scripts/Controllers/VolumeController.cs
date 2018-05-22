using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VolumeController : MonoBehaviour {

    public Slider volumeSlider;

    void Awake()
    {
        if (PlayerPrefs.HasKey(Literals.Saves.MAIN_VOLUME))
            AudioListener.volume = PlayerPrefs.GetFloat(Literals.Saves.MAIN_VOLUME);
        else
            AudioListener.volume = 0.5f;

        volumeSlider.value = AudioListener.volume;
    }

    void Update()
    {      
        PlayerPrefs.SetFloat(Literals.Saves.MAIN_VOLUME, AudioListener.volume);
        AudioListener.volume = volumeSlider.value;
    }
}
