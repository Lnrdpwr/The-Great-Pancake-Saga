using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Очки")]
    [SerializeField] private TMP_Text _scoresText;

    private float _scoresDelta;
    private float _maxScores;

    [Header("Таймер")]
    [SerializeField] private TMP_Text _inGameTimer;

    [Header("Миксер")]
    [SerializeField] private AudioMixer _audioMixer;

    private Animator _patternCanvasAnimator;

    //Панель паузы
    private PausePanel _pausePanel;

    //Панель результатов
    private ResultPanel _panel;
    private int _currentScore = 0;
    private float _timer = 0;
    private bool _finished = false;

    [HideInInspector] public bool KeysUnlocked = false;

    internal static LevelManager Instance;

    private void Awake()
    {
        Instance = this;

        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", -10f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", -10f);

        _audioMixer.SetFloat("SoundVolume", soundVolume);
        _audioMixer.SetFloat("MusicVolume", musicVolume);
    }

    private void Start()
    {
        _panel = ResultPanel.Instance;
        _pausePanel = PausePanel.Instance;

        Pattern.Instance.DisappearEndEvent.AddListener(()=> KeysUnlocked = true);
        Pattern.Instance.DisappearEndEvent.AddListener(() => StartCoroutine(TimerRoutine()));
    }

    private void Update()
    {
        if (_finished)
            return;

        if (Input.GetKeyDown(KeyCode.Escape) && KeysUnlocked)
            _pausePanel.Pause();
        if (Input.GetKeyDown(KeyCode.R) && KeysUnlocked)
            LoadSceneByName(SceneManager.GetActiveScene().name);
    }

    public void UpdateScores(int scores)
    {
        _currentScore += scores;
        _scoresText.text = _currentScore.ToString();
        StartCoroutine(ScoresFade());
    }

    public void Finish()
    {
        _finished = true;

        _pausePanel.UnblockRaycast();

        _panel.gameObject.SetActive(true);
        _panel.UpdatePanel(_timer, _currentScore);

        _scoresText.color = Color.clear;
        _inGameTimer.color = Color.clear;
    }

    public void LockKeyInput()
    {
        _finished = true;
    }

    public void LoadSceneByName(string sceneName)
    {
        Pattern pattern = Pattern.Instance;

        pattern.AppearEndEvent.AddListener(()=>SceneManager.LoadScene(sceneName));
        pattern.Appear();
    }

    IEnumerator TimerRoutine()
    {
        while (!_finished)
        {
            _timer += Time.deltaTime;

            float minutes = Mathf.FloorToInt(_timer / 60);
            float seconds = Mathf.FloorToInt(_timer % 60);
            float miliseconds = _timer * 100 % 100;
            miliseconds = miliseconds >= 99 ? 99 : miliseconds;

            _inGameTimer.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, miliseconds);

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ScoresFade()
    {
        for(float i = 0; i < 0.35f; i += Time.deltaTime)
        {
            _scoresText.color = Color.Lerp(Color.green, Color.white, i / 0.35f);
            yield return new WaitForEndOfFrame();
        }
        
        _scoresText.color = Color.white;
    }
}
