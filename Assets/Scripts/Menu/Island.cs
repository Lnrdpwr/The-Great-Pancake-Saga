using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class Island : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Island Settings")]
    [SerializeField] private float _hoverScale = 1.1f;
    [SerializeField] private float _scaleSpeed = 5f;

    [Header("Level Buttons")]
    [SerializeField] private GameObject _levelButtonsPanel;
    [SerializeField] private Button[] _levelButtons;

    [Header("Parent Object")]
    [SerializeField] private GameObject _islandsParent;

    [Header("Text Settings")]
    [SerializeField] private Color _hoverTextColor = Color.white;
    [SerializeField] private float _colorChangeSpeed = 5f;

    private TMP_Text _islandText;
    private Vector3 _originalScale;
    private bool _isHovered;
    private Color _normalTextColor;

    private void Awake()
    {
        _originalScale = transform.localScale;
        _islandText = GetComponentInChildren<TMP_Text>();

        for (int i = 0; i < _levelButtons.Length; i++)
        {
            int levelIndex = i;
            _levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
        }

        _normalTextColor = _islandText.color;
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
        _levelButtonsPanel.SetActive(true);
        _islandsParent.SetActive(false);
    }

    private void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}