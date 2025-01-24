using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private GameObject _effect;
    [SerializeField] private int _scores;

    private LevelManager _levelManager;

    void Start()
    {
        _levelManager = LevelManager.Instance;
    }

    public int GetScores()
    {
        return _scores;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerMovement player))
        {
            _levelManager.UpdateScores(_scores);
            Instantiate(_effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
