using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private SettingsMenu _setings;

    private void Start()
    {
        _setings.SetAllSettings();
    }

    public void OpenLevelChoice()
    {
        Pattern pattern = Pattern.Instance;

        pattern.AppearEndEvent.AddListener(() => SceneManager.LoadScene("LevelChoice"));
        pattern.Appear();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
