using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Volume")]
    public AudioMixer audioMixer;
    public Slider slider;

    [Header("Resolution")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    private List<Resolution> resolutions = new List<Resolution>();

    private void Start()
    {
        //Initialization
        fullscreenToggle.isOn = Screen.fullScreen;
        resolutionDropdown.ClearOptions();
        float volume = 0f;
        audioMixer.GetFloat("volume", out volume);
        slider.value = volume;

        //Create resolutions and options list
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        int ResolutionIndex = 0;
        for (int i = Screen.resolutions.Length - 1; i > 0; i--)
        {
            Resolution resolution = Screen.resolutions[i];

            if (CheckRepeat(Screen.resolutions[i], resolutions))
                continue;

            resolutions.Add(resolution); 
            options.Add(resolution.width + " x " + resolution.height);

            if(resolution.width == Screen.currentResolution.width &&
                resolution.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = ResolutionIndex;
            }

            ResolutionIndex++;

        }

        //Import options in dropdown
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    private bool CheckRepeat(Resolution checkItem, List<Resolution> list)
    {
        bool isRepeat = false;
        foreach (Resolution resolution in list)
        {
            if(resolution.width == checkItem.width && resolution.height == checkItem.height)
            {
                isRepeat = true;
                break;
            }
        }

        return isRepeat;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void ChangeResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }

}
