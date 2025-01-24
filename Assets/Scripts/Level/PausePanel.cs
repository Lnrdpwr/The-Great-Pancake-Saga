using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private JamThrower _jamThrower;

    internal static PausePanel Instance;

    private void Awake()
    {
        Instance = this;
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _jamThrower = JamThrower.Instance;
    }

    public void Pause()
    {
        _canvasGroup.alpha = 1;
        Time.timeScale = 0;
        _jamThrower?.LockThrow();
    }

    public void UnPause()
    {
        _canvasGroup.alpha = 0;
        Time.timeScale = 1;
        _jamThrower?.UnlockThrow();
    }

    public void OpenMenu()
    {
        LevelManager.Instance.LoadSceneByName("Menu");
        PlayerMovement.Instance.Death();
    }

    public void UnblockRaycast() => _canvasGroup.blocksRaycasts = false;
}
