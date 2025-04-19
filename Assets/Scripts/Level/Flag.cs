using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private AudioClip _finishSound;

    private LevelManager _levelManager;
    private AudioSource _audioSource;
    private bool _finished = false;   

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _levelManager = LevelManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out PlayerMovement player) && !_finished)
            return;
        if(collision.TryGetComponent(out JamThrower jamThrower))
            jamThrower.LockThrow(true);

        LevelManager.Instance.LockKeyInput();
        player.HidePlayer();
        _effect.Play();
        _audioSource.PlayOneShot(_finishSound);
        _finished = true;
        _levelManager.Finish();
    }
}
