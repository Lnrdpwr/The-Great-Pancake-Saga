using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Pattern : MonoBehaviour
{
    [HideInInspector] public UnityEvent AppearEndEvent;
    [HideInInspector] public UnityEvent DisappearEndEvent;

    private Animator _animator;

    internal static Pattern Instance;

    private void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
    }

    public void Appear()
    {
        _animator.SetTrigger("Appear");

        Button[] allButtons = FindObjectsOfType<Button>();

        foreach (Button button in allButtons)
        {
            button.interactable = false;
        }
    }

    public void CallAppearEndEvent()
    {
        Time.timeScale = 1;
        AppearEndEvent.Invoke();
    }

    public void CallDisappearEndEvent() => DisappearEndEvent.Invoke();
}
