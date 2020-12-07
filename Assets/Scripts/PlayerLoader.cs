using UnityEngine;
using UnityEngine.Networking;


public class PlayerLoader : NetworkBehaviour
{
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private PlayerController _playerController;

    [SyncVar(hook = "HookUnitIdentity")] private NetworkIdentity _unitIdentity;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        // если игрок запустился на сервере
        if (isServer)
        {
            GameObject unit = Instantiate(_unitPrefab, transform.position, Quaternion.identity);
            NetworkServer.Spawn(unit);
            _unitIdentity = unit.GetComponent<NetworkIdentity>();
            _playerController.SetCharacter(unit.GetComponent<Character>(), true);
        }
        // если игрок запустился на локальной машине
        else
        {
            CmdCreatePlayer();
        }
    }

    // [Command] говорит нам о том, что это метод нужно выполнить на сервере
    // Cmd в названии метода обязательно, иначе не скомпилируется
    [Command]
    public void CmdCreatePlayer()
    {
        GameObject unit = Instantiate(_unitPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(unit);
        _unitIdentity = unit.GetComponent<NetworkIdentity>();
        _playerController.SetCharacter(unit.GetComponent<Character>(), false);
    }

    [ClientCallback]
    private void HookUnitIdentity(NetworkIdentity unit)
    {
        if (isLocalPlayer)
        {
            _unitIdentity = unit;
            _playerController.SetCharacter(unit.GetComponent<Character>(), true);
        }
    }
}