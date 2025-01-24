using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JamThrower : MonoBehaviour
{
    [Header("Варенье")]
    [SerializeField] private GameObject _jam;
    [SerializeField] private Image _jamImage;
    [SerializeField] private float _reloadTime;

    /*
    [Header("Звук")]
    [SerializeField] private AudioClip _jamReloadSound;
    */

    private AudioSource _audioSource;
    private bool _canThrow = true;

    internal static JamThrower Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_canThrow)
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Instantiate(_jam, transform.position, Quaternion.Euler(0, 0, angle));

            _canThrow = false;
            StartCoroutine(Reload());
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            Instantiate(_jam, transform.position, Quaternion.Euler(0, 0, -90));

            _canThrow = false;
            StartCoroutine(Reload());
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(_jam, transform.position, Quaternion.Euler(0, 0, 90));

            _canThrow = false;
            StartCoroutine(Reload());
        }                
    }

    public void LockThrow(bool lockForever = false)
    {
        _canThrow = false;

        if (lockForever)
            StopAllCoroutines();
    }

    public void UnlockThrow()
    {
        _canThrow = true;
    }

    IEnumerator Reload()
    {
        for(float i = 0; i < _reloadTime; i += Time.deltaTime)
        {
            _jamImage.fillAmount = i / _reloadTime;
            yield return new WaitForEndOfFrame();
        }

        //_audioSource.PlayOneShot(_jamReloadSound);
        _jamImage.fillAmount = 1;
        _canThrow = true;
    }
}
