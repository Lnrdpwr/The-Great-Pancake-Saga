using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _lookDownOffset;
    [SerializeField] private float _smoothTime;

    private Vector3 _velocity;
    private bool _lookingDown = false;

    private void LateUpdate()
    {
        if (Input.GetAxisRaw("Vertical") == -1)
            _lookingDown = true;
        else if(Input.GetAxisRaw("Vertical") == 0)
            _lookingDown = false;

        Vector3 offset = _lookingDown ? _lookDownOffset : _offset;  

        transform.position = Vector3.SmoothDamp(transform.position, 
            _target.position + offset, 
            ref _velocity, 
            _smoothTime);
    }
}
