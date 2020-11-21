using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _movementMask;

    private Camera _camera;
    private UnitMotor _motor;
    private Ray _ray;
    private RaycastHit _hit;

    private void Start()
    {
        _camera = Camera.main;
        _motor = GetComponent<UnitMotor>();
        _camera.GetComponent<CameraController>().Target = transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 100.0f, _movementMask))
            {
                _motor.MoveToPoint(_hit.point);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 100.0f))
            {
                //todo
            }
        }
    }
}