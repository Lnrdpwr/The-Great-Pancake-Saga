using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class Island : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Island Settings")]
    [SerializeField] private AudioClip _islandTheme;
    [SerializeField] private float _hoverScale = 1.1f;
    [SerializeField] private float _scaleSpeed = 5f;
    [SerializeField] private int _firstLevelIndex;

    [Header("Level Buttons")]
    [SerializeField] private GameObject _levelButtonsPanel;
    [SerializeField] private Button[] _levelButtons;

    [Header("Parent Object")]
    [SerializeField] private GameObject _islandsParent;

    [Header("Text Settings")]
    [SerializeField] private Color _hoverTextColor = Color.white;
    [SerializeField] private float _colorChangeSpeed = 5f;

    [Header("Animation Settings")]
    [SerializeField] private float _zoomDuration = 1f;
    [SerializeField] private float _zoomScale = 3f;
    [SerializeField] private float _bounceHeight = 10f;
    [SerializeField] private float _bounceSpeed = 2f;

    private TMP_Text _islandText;
    private Vector3 _originalScale;
    private bool _isHovered;
    private Color _normalTextColor;
    private CanvasGroup _islandsCanvasGroup;
    private CanvasGroup _levelButtonsCanvasGroup;
    private Vector3 _originalPosition;
    private Vector3 _originalLocalPosition;
    private bool _isBouncing = true;

    private void Awake()
    {
        int currentLevel = PlayerPrefs.GetInt("LevelPassed", 0)+1;
        MusicManager musicManager = FindObjectOfType<MusicManager>();

        _originalScale = transform.localScale;
        _islandText = GetComponentInChildren<TMP_Text>();

        _islandsCanvasGroup = _islandsParent.GetComponent<CanvasGroup>();
        _levelButtonsCanvasGroup = _levelButtonsPanel.GetComponent<CanvasGroup>();

        for (int i = 0; i < _levelButtons.Length; i++)
        {
            int levelIndex = i + _firstLevelIndex;
            _levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
            _levelButtons[i].onClick.AddListener(() => musicManager.Transition(_islandTheme));

            if(levelIndex > currentLevel) {
                _levelButtons[i].interactable = false;
            }
        }

        _originalPosition = transform.position;
        _originalLocalPosition = transform.localPosition;
        _normalTextColor = _islandText.color;
        _levelButtonsPanel.transform.localScale = Vector3.zero;
        _levelButtonsCanvasGroup.alpha = 0;
    }

    private void Update()
    {
        if (_isHovered)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                _originalScale * _hoverScale,
                _scaleSpeed * Time.deltaTime
            );
        }
        else
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                _originalScale,
                _scaleSpeed * Time.deltaTime
            );
        }

        _islandText.color = Color.Lerp(
            _islandText.color,
            _isHovered ? _hoverTextColor : _normalTextColor,
            _colorChangeSpeed * Time.deltaTime
        );

        if (_isBouncing)
        {
            BounceIsland();
        }
    }

    private void BounceIsland()
    {
        float bounce = Mathf.Sin(Time.time * _bounceSpeed) * _bounceHeight;
        transform.localPosition = _originalLocalPosition + new Vector3(0, bounce, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovered = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartZoomAnimation();
    }

    private void StartZoomAnimation()
    {
        _islandsCanvasGroup.interactable = false;
        _islandsCanvasGroup.blocksRaycasts = false;

        _isBouncing = false;

        Vector3 centerPosition = GetCenterPosition();
        LeanTween.move(gameObject, centerPosition, _zoomDuration)
            .setEase(LeanTweenType.easeOutQuad);

        LeanTween.scale(gameObject, _originalScale * _zoomScale, _zoomDuration)
            .setEase(LeanTweenType.easeOutQuad);

        LeanTween.alphaCanvas(_islandsCanvasGroup, 0, _zoomDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                _islandsParent.SetActive(false);
                _levelButtonsCanvasGroup.interactable = true;
                _levelButtonsCanvasGroup.blocksRaycasts = true;
            });

        _levelButtonsPanel.SetActive(true);
        LeanTween.scale(_levelButtonsPanel, Vector3.one, _zoomDuration)
            .setEase(LeanTweenType.easeOutQuad);

        LeanTween.alphaCanvas(_levelButtonsCanvasGroup, 1, _zoomDuration)
            .setEase(LeanTweenType.easeOutQuad);
    }

    public void OnBackButtonClicked()
    {
        _levelButtonsCanvasGroup.interactable = false;
        _levelButtonsCanvasGroup.blocksRaycasts = false;

        LeanTween.scale(_levelButtonsPanel, Vector3.zero, _zoomDuration)
            .setEase(LeanTweenType.easeOutQuad);

        LeanTween.alphaCanvas(_levelButtonsCanvasGroup, 0, _zoomDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                _levelButtonsPanel.SetActive(false);
            });

        _islandsParent.SetActive(true);

        LeanTween.moveLocal(gameObject, _originalLocalPosition, _zoomDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                _isBouncing = true;
                _isHovered = false;
                _islandsCanvasGroup.interactable = true;
                _islandsCanvasGroup.blocksRaycasts = true;
            });

        LeanTween.alphaCanvas(_islandsCanvasGroup, 1, _zoomDuration)
            .setEase(LeanTweenType.easeOutQuad);

        LeanTween.scale(gameObject, _originalScale, _zoomDuration)
            .setEase(LeanTweenType.easeOutQuad);
    }

    private Vector3 GetCenterPosition()
    {
        Vector3 center = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        center.z = transform.position.z;
        return center;
    }

    private void LoadLevel(int levelIndex)
    {
        Pattern pattern = Pattern.Instance;

        pattern.AppearEndEvent.AddListener(() => SceneManager.LoadScene($"Level{levelIndex}"));
        pattern.Appear();
    }
}