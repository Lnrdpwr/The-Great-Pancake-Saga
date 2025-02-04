using System.Collections;
using UnityEngine;

public class Jam : MonoBehaviour
{
    [Header("Джем")]
    [SerializeField] private GameObject _effects;
    [SerializeField] private float _speed;
    [SerializeField] private float _invularableTime;

    [Header("Взрыв")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;

    [Header("Звуки")]
    [SerializeField] private AudioClip _explosionSound;

    private AudioSource _jamAudio;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;
    private bool _invularable = false;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _jamAudio = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = transform.right * _speed;
        _rigidbody.AddTorque(Random.Range(-360, 360));

        Destroy(gameObject, 7);
    }

    void SetOffInvularability()
    {
        _invularable = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ground") && !collision.CompareTag("Walls"))
            return;
        if (_invularable)
            return;
        if (_rigidbody.velocity.y > 0 && collision.CompareTag("Ground"))
        {
            _invularable = true;
            Invoke("SetOffInvularability", 0.85f);
            return;
        }

        Collider2D player = Physics2D.OverlapCircle(transform.position, _explosionRadius, _playerLayer);
        if (player != null)
        {
            Vector2 dir = player.transform.position - transform.position;
            player.GetComponent<PlayerMovement>().ForcedMove(dir, _explosionForce, 0);
        }

        _jamAudio.PlayOneShot(_explosionSound);
        _renderer.color = Color.clear;
        _invularable = true;
        Instantiate(_effects, transform.position, Quaternion.identity);
        StartCoroutine(WaitForSound());
    }

    IEnumerator WaitForSound()
    {
        yield return new WaitUntil(()=> !_jamAudio.isPlaying);
        Destroy(gameObject);
    }
}
