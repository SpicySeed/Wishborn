using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Dropdown resDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundsVolumeSlider;

    private Resolution[] resolutions = null;

    private FMOD.Studio.Bus music;
    private FMOD.Studio.Bus soundEffects;
    private float musicVolume = 0;
    private float soundsVolume = 0;

    public void Awake()
    {
        resolutions = Screen.resolutions;
        LoadResolutions();

        music = FMODUnity.RuntimeManager.GetBus("bus:/MUSIC");
        soundEffects = FMODUnity.RuntimeManager.GetBus("bus:/SoundEffects");

        fullScreenToggle.isOn = Screen.fullScreen;

        music.getVolume(out musicVolume);
        music.getVolume(out soundsVolume);
        musicVolumeSlider.value = musicVolume;
        soundsVolumeSlider.value = soundsVolume;

        resDropdown.onValueChanged.AddListener(delegate { CheckResolutionDropdown(); });
        fullScreenToggle.onValueChanged.AddListener(delegate { CheckFullscreenToggle(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { CheckVolumeSlider(); });
        soundsVolumeSlider.onValueChanged.AddListener(delegate { CheckVolumeSlider(); });
    }

    private void LoadResolutions()
    {
        resDropdown.ClearOptions();
        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].Equals(Screen.currentResolution))
                currentIndex = i;
            resDropdown.options.Add(new Dropdown.OptionData(resolutions[i].ToString()));
        }

        resDropdown.value = currentIndex;
    }

    private void CheckResolutionDropdown()
    {
        int index = resDropdown.value;
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
        Debug.Log("Resolution: " + resolutions[index].ToString());
    }

    private void CheckFullscreenToggle()
    {
        Screen.fullScreen = fullScreenToggle.isOn;
        Debug.Log("Fullscreen: " + fullScreenToggle.isOn.ToString());
    }

    private void CheckVolumeSlider()
    {
        music.setVolume(musicVolumeSlider.value);
        soundEffects.setVolume(soundsVolumeSlider.value);
        Debug.Log("Volume: " + musicVolumeSlider.value.ToString());
        Debug.Log("Volume: " + soundsVolumeSlider.value.ToString());
    }
}
