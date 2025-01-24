using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("«вук и музыка")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;

    private void Start()
    {
        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.7f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);

        _soundSlider.value = soundVolume;
        _musicSlider.value = musicVolume;

        Debug.Log(soundVolume);

        _audioMixer.SetFloat("SoundVolume", Mathf.Log10(soundVolume) * 20);
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);

        gameObject.SetActive(false);
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
        PlayerPrefs.SetFloat("SoundVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}
