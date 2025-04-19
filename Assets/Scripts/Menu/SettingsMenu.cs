using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("«вук и музыка")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;

    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown _dropdown;

    private Resolution[] _resolutions;
    private List<Resolution> _sortedResolutions = new List<Resolution>();
    private RefreshRate _currentRefreshRate;
    private int _currentRefreshRateIndex;

    private CanvasGroup _canvasGroup;
    private FullScreenMode _screenMode;

    private void Start()
    {
        //Volume
        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", -10f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", -10f);

        _soundSlider.value = Mathf.Pow(10, soundVolume / 20);
        _musicSlider.value = Mathf.Pow(10, soundVolume / 20);

        _audioMixer.SetFloat("SoundVolume", soundVolume);
        _audioMixer.SetFloat("MusicVolume", musicVolume);

        //Screen mode
        /*switch(PlayerPrefs.GetInt("ScreenMode", 0))
        {
            case 0:
                _screenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                _screenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                _screenMode = FullScreenMode.Windowed;
                break;
        }

        Screen.fullScreenMode = _screenMode;*/

        //ScreenResolution
        _resolutions = Screen.resolutions;
        
        for(int i = 0; i < _resolutions.Length; i++)
        {
            if (_resolutions[i].height <= Screen.height && 
                _resolutions[i].width <= Screen.width && 
                _resolutions[i].refreshRateRatio.value <= Screen.currentResolution.refreshRateRatio.value) { 

                    _sortedResolutions.Add(_resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for(int i = 0; i < _sortedResolutions.Count; i++)
        {
            string option = $"{_sortedResolutions[i].width}x{_sortedResolutions[i].height} {(int)_sortedResolutions[i].refreshRateRatio.value}Hz";
            options.Add(option);

            if (_resolutions[i].height == Screen.height &&
                _resolutions[i].width == Screen.width &&
                _resolutions[i].refreshRateRatio.value == Screen.currentResolution.refreshRateRatio.value)
            {

                _currentRefreshRateIndex = i;
            }
        }

        _dropdown.ClearOptions();
        _dropdown.AddOptions(options);
        _dropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        Resolution resolution = _sortedResolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.ExclusiveFullScreen, resolution.refreshRateRatio);
    }

    public void SetAllSettings()
    {
        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.7f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);

        _audioMixer.SetFloat("SoundVolume", Mathf.Log10(soundVolume) * 20);
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
    }

    public void SetSoundVolume(float volume)
    {
        _audioMixer.SetFloat("SoundVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SoundVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void ChangeResolution(int x, int y)
    {
        Screen.SetResolution(x, y, _screenMode);
    }

    public void ToggleFullscreen(bool isOn)
    {
        Screen.fullScreen = isOn;
    }
}
