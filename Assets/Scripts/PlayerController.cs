using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;


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
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        _ray = _camera.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(_ray, out _hit, 100.0f, _movementMask))
                        {
                            CmdSetMovePoint(_hit.point);
                        }
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        _ray = _camera.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(_ray, out _hit, 100.0f, ~(1 << LayerMask.NameToLayer("Player"))))
                        {
                            Interactable interactable = _hit.collider.GetComponent<Interactable>();
                            if (interactable != null)
                            {
                                CmdSetFocus(interactable.GetComponent<NetworkIdentity>());
                            }
                        }
                    }
                }
                //использование навыков
                if (Input.GetButtonDown("Skill1")) CmdUseSkill(0);
                if (Input.GetButtonDown("Skill2")) CmdUseSkill(1);
                if (Input.GetButtonDown("Skill3")) CmdUseSkill(2);
            }
        }
    }

    public void SetCharacter(Character character, bool isLocalPlayer)
    {
        _character = character;
        if (isLocalPlayer)
        {
            _camera.GetComponent<CameraController>().Target = character.transform;
            SkillsPanel.Instance.SetSkills(character.unitSkills);
        }
    }

    [Command]
    private void CmdSetFocus(NetworkIdentity newFocus)
    {
        if (!_character.unitSkills.InCast)
        {
            _character.SetNewFocus(newFocus.GetComponent<Interactable>());
        }
    }

    [Command]
    private void CmdSetMovePoint(Vector3 point)
    {
        if (!_character.unitSkills.InCast)
        {
            _character.SetMovePoint(point);
        }
    }

    [Command]
    private void CmdUseSkill(int skillNum)
    {
        if (!_character.unitSkills.InCast)
        {
            _character.UseSkill(skillNum);
        }
    }
}