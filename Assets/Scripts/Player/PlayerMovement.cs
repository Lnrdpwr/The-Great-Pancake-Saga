using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    #region Movement
    [Header("Скорость")]
    [SerializeField] private float _speed;

    private bool _facingRight = true;

    [Header("Прыжок")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpTime;
    [SerializeField] private float _jumpMultiplier;
    [SerializeField] private float _fallMultiplier;

    private Vector2 _gravity;
    private float _jumpTimer;
    private bool _isJumping;

    [Header("Койот-джамп")]
    [SerializeField] private float _koyotTime = 0.1f;

    float _koyotTimer = 0f;
    bool _koyotAvailable = true;

    [Header("Деш")]
    [SerializeField] private GameObject _dashTrail;
    [SerializeField] private CanvasGroup _barGroup;
    [SerializeField] private Image _reloadBar;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashReload;

    private Coroutine _dashRoutine;
    private bool _dashReloaded = true;

    #endregion

    #region Beauty

    [Header("Анимации")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _deathEffect;

    [Header("Звуки")]
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _dashSound;
    [SerializeField] private AudioClip _deathSound;

    private AudioSource _playerAudio;

    #endregion

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private bool _canMove = false;

    internal static PlayerMovement Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerAudio = GetComponent<AudioSource>();
        _gravity = new Vector2(0, -Physics2D.gravity.y);

        Pattern.Instance.DisappearEndEvent.AddListener(()=> _canMove = true);
    }

    private void Update()
    {
        if (!_canMove) return;

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && (OnGround() || _koyotTime > _koyotTimer) && !_isJumping)
        {
            _koyotAvailable = false;
            _koyotTimer = _koyotTime;

            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            _isJumping = true;
            _jumpTimer = 0;
            _playerAudio.PlayOneShot(_jumpSound);
        }
        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            _koyotAvailable = false;
            _koyotTimer = _koyotTime;

            _isJumping = false;
            _jumpTimer = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && _dashReloaded)
        {
            _animator.SetTrigger("Dash");
            _animator.SetBool("isDashing", true);

            if(_dashRoutine != null)
                StopCoroutine(_dashRoutine);
            _dashRoutine = StartCoroutine(Dash());
            _playerAudio.PlayOneShot(_dashSound);

            _dashReloaded = false;
            _canMove = false;
        }

        if (_koyotTimer < _koyotTime && !_koyotAvailable)
            _koyotTimer += Time.deltaTime;

    }

    private void FixedUpdate()
    {
        if (!_canMove) return;

        //Движение
        float horInput = Input.GetAxisRaw("Horizontal");
        Vector2 velocity = new Vector2(horInput * _speed, _rigidbody.velocity.y);
        _rigidbody.velocity = velocity;

        if (OnGround())
        {
            _koyotAvailable = true;
            
        }
        else if(_koyotAvailable)
        {
            _koyotAvailable = false;
            _koyotTimer = 0;
        }
        

        Flip(horInput);

        //Прыжок
        if (_rigidbody.velocity.y > 0 && _isJumping)
        {
            _jumpTimer += Time.deltaTime;
            if (_jumpTimer > _jumpTime)
                _isJumping = false;

            _rigidbody.velocity += _gravity * _jumpMultiplier * Time.fixedDeltaTime;
        }
        else if (_rigidbody.velocity.y < 0)
            _rigidbody.velocity -= _gravity * _fallMultiplier * Time.fixedDeltaTime;

        //Анимации
        if (_rigidbody.velocity.x != 0)
            _animator.SetBool("isMoving", true);
        else
            _animator.SetBool("isMoving", false);

        if (!OnGround())
        {
            _animator.SetTrigger("Jump");
            _animator.SetBool("isJumping", true);
        }
        else
            _animator.SetBool("isJumping", false);
    }

    private void Flip(float moveDir)
    {
        if(_facingRight && moveDir < 0)
        {
            _facingRight = false;
            _spriteRenderer.flipX = true;
        }
        else if(!_facingRight && moveDir > 0)
        {
            _facingRight = true;
            _spriteRenderer.flipX = false;
        }
    }

    public void ForcedMove(Vector2 direction, float force, float lockTime = 0.1f)
    {
        _canMove = false;
        Invoke("UnlockMovement", lockTime);

        _rigidbody.AddForce(direction.normalized * force);
    }

    private void UnlockMovement()
    {
        _canMove = true;
    }

    public void FreezePlayer()
    {
        _animator.SetBool("isMoving", false);
        _animator.SetBool("isJumping", false);
        _animator.SetBool("isDashing", false);

        if (_dashRoutine != null)
        {
            StopCoroutine(_dashRoutine);
            _barGroup.alpha = 0;
        }

        _canMove = false;
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }

    public void Death()
    {
        FreezePlayer();
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.gravityScale = 0;
        _rigidbody.isKinematic = true;

        _playerAudio.PlayOneShot(_deathSound);

        _animator.SetTrigger("Death");
        _animator.SetBool("isDead", true);
        Instantiate(_deathEffect, transform.position, Quaternion.identity);
    }

    private bool OnGround() => Physics2D.OverlapCircle(_groundCheck.position, 0.33f, _groundLayer);

    IEnumerator Dash()
    {
        int dir = _facingRight ? 1 : -1;
        float lastGravity = _rigidbody.gravityScale;

        GameObject trail = Instantiate(_dashTrail, transform);
        Vector2 dashVelocity = dir * _dashSpeed * Vector2.right;
        
        _rigidbody.gravityScale = 0;

        transform.position += new Vector3(0, 0.025f);

        for(float i = 0; i < _dashTime; i += Time.deltaTime)
        {
            _rigidbody.velocity = dashVelocity;
            yield return new WaitForEndOfFrame();
        }

        _rigidbody.gravityScale = lastGravity;
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        if(trail)
            trail.transform.parent = null;
        _animator.SetBool("isDashing", false);

        _canMove = true;
        _koyotAvailable = false;

        _barGroup.alpha = 1;
        for (float i = 0; i < _dashReload; i += Time.deltaTime)
        {
            _reloadBar.fillAmount = i / _dashReload;
            yield return new WaitForEndOfFrame();
        }

        _reloadBar.fillAmount = 1;
        _dashReloaded = true;

        for (float i = 0; i < 0.1f; i += Time.deltaTime)
        {
            _barGroup.alpha = 1 - i / 0.25f;
            yield return new WaitForEndOfFrame();
        }
        _barGroup.alpha = 0;
    }
}
