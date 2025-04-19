using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class MenuPhase : MonoBehaviour
{
    public int _deaths;
    [SerializeField] private Image _image;
    [SerializeField] private MusicManager _musicManager;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Sprite _phase1;
    [SerializeField] private Sprite _phase2;
    [SerializeField] private Sprite _phase3;

    [SerializeField] private Light _light1;
    [SerializeField] private Light _light2;

    [SerializeField] private AudioClip _default;
    [SerializeField] private AudioClip _audio1;
    [SerializeField] private AudioClip _audio2;

    [SerializeField] private GameObject _button;
    void Start()
    {
        _deaths = PlayerPrefs.GetInt("Deaths", 0);
        if (_deaths >= 25)
        {
            _light1.color = new Color(227, 208, 184, 255);
            _light2.intensity = 6.8f;
            _image.sprite = _phase2;
            _musicManager._defaultTrack = _audio1;
            _audioSource.clip = _audio1;
        }
        if (_deaths >= 50)
        {
            _light1.color = new Color(186, 185, 185, 255);
            _light2.intensity = 3.8f;
            _image.sprite = _phase3;
            _musicManager._defaultTrack = _audio2;
            _audioSource.clip = _audio2;
            _button.SetActive(true);
        }
        if (_deaths < 25)
        {
            _light1.color = new Color(255, 214, 161, 255);
            _image.sprite = _phase1;
            _light2.intensity = 10.8f;
            _musicManager._defaultTrack = _default;
            _audioSource.clip = _default;
            _button.SetActive(false);
        }
    }
}
