using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LevelChoice : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    private void Start()
    {
        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", -10f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", -10f);

        _audioMixer.SetFloat("SoundVolume", soundVolume);
        _audioMixer.SetFloat("MusicVolume", musicVolume);
    }

    public void LoadLevel(string index)
    {
        Pattern pattern = Pattern.Instance;
        string levelName = $"Level{index}";

        pattern.AppearEndEvent.AddListener(() => SceneManager.LoadScene(levelName));
        pattern.Appear();
    }

    public void LoadMenu()
    {
        Pattern pattern = Pattern.Instance;

        pattern.AppearEndEvent.AddListener(()=> SceneManager.LoadScene("Menu"));
        pattern.Appear();
    }
}
