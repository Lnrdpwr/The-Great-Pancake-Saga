using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] _destionations;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _waitTime;

    private Transform _player;
    private Coroutine _landingRoutine;
    private int _currentIndex;

    private void Start()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        if(_currentIndex == _destionations.Length)
        {
            _currentIndex = 0;
        }


        LeanTween.move(gameObject, _destionations[_currentIndex], _moveDuration)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(()=>
            {
                _currentIndex++;
                Invoke("MovePlatform", _waitTime);
            });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement player) && player.isOnGround)
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement player))
        {
            collision.transform.parent = null;
        }
    }
}
