using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{

    // for managing sound https://www.youtube.com/watch?v=yWCHaTwVblk
    public Slider volumeSlider;
    public Slider soundFXSlider;

    public enum AudioTypes {soundFX, music}
    public AudioTypes audioType;

    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup soundFXMixerGroup;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume")) {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        } else {
            Load();
        }
    }

    public void ChangeVolume() {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load() {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save() {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
