using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _amountOfParallax;

    private Camera _mainCamera;
    private Vector3 _startingPos;
    
    void Start()
    {
        _startingPos = transform.position;
        _mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 position = _mainCamera.transform.position;
        position.z = 0;
        Vector3 distance = new Vector3(position.x * _amountOfParallax, position.y, 0);
        Vector3 newPosition = _startingPos + distance;

        transform.position = newPosition;
    }
}




