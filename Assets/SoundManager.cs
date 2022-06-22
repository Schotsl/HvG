using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{

    // for managing sound https://www.youtube.com/watch?v=yWCHaTwVblk
    public Slider musicSlider;
    public Slider soundFXSlider;
    public AudioMixer masterMixer;


    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume") && !PlayerPrefs.HasKey("soundFXVolume")) {
            PlayerPrefs.SetFloat("musicVolume", 1);
            PlayerPrefs.SetFloat("soundFXVolume", 1);
            Load();
        } else if (!PlayerPrefs.HasKey("musicVolume")) {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        } else if (!PlayerPrefs.HasKey("soundFXVolume")) {
            PlayerPrefs.SetFloat("soundFXVolume", 1);
            Load();
        } else {
            Load();
        }
    }

    public void ChangeVolume() {
        masterMixer.SetFloat("musicVolume", Mathf.Log10(musicSlider.value) * 20);
        masterMixer.SetFloat("soundFXVolume", Mathf.Log10(soundFXSlider.value) * 20);
        Save();
    }

    private void Load() {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        soundFXSlider.value = PlayerPrefs.GetFloat("soundFXVolume");
    }

    private void Save() {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("soundFXVolume", soundFXSlider.value);
    }
}
