using UnityEngine;
using UnityEngine.Networking;


[System.Obsolete]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private LayerMask _movementMask;

    private Camera _camera;
    private Character _character;

    private Ray _ray;
    private RaycastHit _hit;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            if (_character != null)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _ray = _camera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(_ray, out _hit, 100.0f, _movementMask))
                    {
                        CmdSetMovePoint(_hit.point);
                    }
                }
            }
        }
    }

    public void SetCharacter(Character character, bool isLocalPlayer)
    {
        _character = character;
        if (isLocalPlayer)
        {
            _camera.GetComponent<CameraController>().Target = character.transform;
        }
    }

    [Command]
    public void CmdSetMovePoint(Vector3 point)
    {
        _character.SetMovePoint(point);
    }

    private void OnDestroy()
    {
        if (_character != null)
        {
            Destroy(_character.gameObject);
        }
    }
}