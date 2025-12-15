using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class VolumeSettings : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer mainMixer;

    [Header("Sliderlar")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Sayı Yazıları")]
    public TextMeshProUGUI masterText;
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI sfxText;

    void Start()
    {
        float savedMaster = PlayerPrefs.GetFloat("MasterVolume", 100);
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 100);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 100);
        masterSlider.value = savedMaster;
        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;
        SetMasterVolume(savedMaster);
        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);
    }
    public void SetMasterVolume(float value)
    {
        if(masterText != null) masterText.text = value.ToString("0");
        float volumeInDecibels = (value <= 1) ? -80f : Mathf.Log10(value / 100f) * 20f;
        
        mainMixer.SetFloat("MasterVol", volumeInDecibels);

        // 3. Kaydet
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        if(musicText != null) musicText.text = value.ToString("0");

        float volumeInDecibels = (value <= 1) ? -80f : Mathf.Log10(value / 100f) * 20f;
        mainMixer.SetFloat("MusicVol", volumeInDecibels);

        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        if(sfxText != null) sfxText.text = value.ToString("0");

        float volumeInDecibels = (value <= 1) ? -80f : Mathf.Log10(value / 100f) * 20f;
        mainMixer.SetFloat("SFXVol", volumeInDecibels);

        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}