using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target = null;
    [SerializeField] private Vector3 _offset = Vector3.zero;
    [SerializeField] private float _zoomSpeed = 4.0f;
    [SerializeField] private float _minZoom = 2.0f;
    [SerializeField] private float _maxZoom = 15.0f;
    [SerializeField] private float _pitch = 1.0f;

    private float _currentZoom = 10.0f;
    private float _currentRoot = 0.0f;
    private float _prevMouseX;

    public Transform Target
    {
        set
        {
            _target = value;
        }
    }

    private void Update()
    {
        if (_target != null)
        {
            _currentZoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);

            if (Input.GetMouseButton(2))
            {
                _currentRoot += Input.mousePosition.x - _prevMouseX;
            }
        }
        _prevMouseX = Input.mousePosition.x;
    }

    private void LateUpdate()
    {
        if (_target != null)
        {
            transform.position = _target.position - _offset * _currentZoom;
            transform.LookAt(_target.position + Vector3.up * _pitch);
            transform.RotateAround(_target.position, Vector3.up, _currentRoot);
        }
    }
}