using UnityEngine;
using UnityEngine.Networking;


public class PlayerLoader : NetworkBehaviour
{
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Player _player;

    [SyncVar(hook = "HookUnitIdentity")] private NetworkIdentity _unitIdentity;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        // если игрок запустился на сервере
        if (isServer)
        {
            Character character = CreateCharacter();
            _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), true);
            _playerController.SetCharacter(character, true);
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
        Character character = CreateCharacter();
        _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), false);
        _playerController.SetCharacter(character, false);
    }

    [ClientCallback]
    private void HookUnitIdentity(NetworkIdentity unit)
    {
        if (isLocalPlayer)
        {
            _unitIdentity = unit;
            Character character = unit.GetComponent<Character>();
            _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), true);
            _playerController.SetCharacter(character, true);
        }
    }

    public Character CreateCharacter()
    {
        GameObject unit = Instantiate(_unitPrefab/*, transform.position, Quaternion.identity*/);
        NetworkServer.Spawn(unit);
        _unitIdentity = unit.GetComponent<NetworkIdentity>();
        return unit.GetComponent<Character>();
    }

    public override bool OnCheckObserver(NetworkConnection connection)
    {
        return false;
    }
}