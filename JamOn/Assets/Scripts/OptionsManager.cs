using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Dropdown resDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Slider volumeSlider;

    private Resolution[] resolutions = null;

    public void Awake()
    {
        resolutions = Screen.resolutions;
        LoadResolutions();

        fullScreenToggle.isOn = Screen.fullScreen;
        volumeSlider.value = AudioListener.volume;

        resDropdown.onValueChanged.AddListener(delegate { CheckResolutionDropdown(); });
        fullScreenToggle.onValueChanged.AddListener(delegate { CheckFullscreenToggle(); });
        volumeSlider.onValueChanged.AddListener(delegate { CheckVolumeSlider(); });
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
        AudioListener.volume = volumeSlider.value;
        Debug.Log("Volume: " + volumeSlider.value.ToString());
    }
}
