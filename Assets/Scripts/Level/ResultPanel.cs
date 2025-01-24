using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _resultScore;
    [SerializeField] private TMP_Text _bestScore;
    [SerializeField] private TMP_Text _resultTime;
    [SerializeField] private TMP_Text _bestTime;
    [SerializeField] private Animator[] _stars;

    [Header("Уровень")]
    [SerializeField] private int _levelIndex;

    private Animator _animator;
    private int _maxScores;
    private int _scoresDelta;

    internal static ResultPanel Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();

        Collectable[] allHoney = FindObjectsOfType<Collectable>();
        foreach (Collectable honey in allHoney)
            _maxScores += honey.GetScores();

        _scoresDelta = _maxScores / 5;
    }

    public void UpdatePanel(float time, int score)
    {
        _animator.SetTrigger("Appear");

        //Level result

        float minutes = Mathf.FloorToInt(time / 100);
        float seconds = Mathf.FloorToInt(time % 100);
        float miliseconds = time * 100 % 99;

        _resultScore.text = $"Score: {score}";
        _resultTime.text = string.Format("Time: {0:00}:{1:00}:{2:00}", minutes, seconds, miliseconds);

        //Best result

        int bestScore = PlayerPrefs.GetInt($"BestScore{_levelIndex}", 0);
        float bestTime = PlayerPrefs.GetFloat($"BestTime{_levelIndex}", 0);

        if(bestScore < score)
        {
            bestScore = score;
            PlayerPrefs.SetInt($"BestScore{_levelIndex}", score);
        }
        if(!PlayerPrefs.HasKey($"BestTime{_levelIndex}") || time < bestTime)
        {
            bestTime = time;
            PlayerPrefs.SetFloat($"BestTime{_levelIndex}", time);
        }    

        minutes = Mathf.FloorToInt(bestTime / 100);
        seconds = Mathf.FloorToInt(bestTime % 100);
        miliseconds = bestTime * 100 % 99;

        _bestScore.text = $"Best score: {bestScore}";
        _bestTime.text = string.Format("Best time: {0:00}:{1:00}:{2:00}", minutes, seconds, miliseconds);

        if (score != 0)
            StartCoroutine(StarsAppearr(score));
    }

    public void Restart()
    {
        LevelManager.Instance.LoadSceneByName(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        string sceneName = $"Level{_levelIndex + 1}";
        LevelManager.Instance.LoadSceneByName(sceneName);
    }

    public void LoadMenu()
    {
        LevelManager.Instance.LoadSceneByName("Menu");
    }

    IEnumerator StarsAppearr(int score)
    {
        for (int i = 0; i < 5; i++)
        {
            if (_scoresDelta * (i+1) <= score)
            {
                _stars[i].SetTrigger("Appear");
                yield return new WaitForSeconds(0.25f);
            }
            else
                break;
        }
    }
}
