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
            Character character = CreateCharacter();
            _playerController.SetCharacter(character, true);
            InventoryUi.instance.SetInventory(character.inventory);
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
        _playerController.SetCharacter(CreateCharacter(), false);
    }

    [ClientCallback]
    private void HookUnitIdentity(NetworkIdentity unit)
    {
        if (isLocalPlayer)
        {
            _unitIdentity = unit;
            Character character = unit.GetComponent<Character>();
            _playerController.SetCharacter(character, true);
            character.SetInventory(GetComponent<Inventory>());
            InventoryUi.instance.SetInventory(character.inventory);
        }
    }

    public Character CreateCharacter()
    {
        GameObject unit = Instantiate(_unitPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(unit);
        _unitIdentity = unit.GetComponent<NetworkIdentity>();
        unit.GetComponent<Character>().SetInventory(GetComponent<Inventory>());
        return unit.GetComponent<Character>();
    }

    public override bool OnCheckObserver(NetworkConnection connection)
    {
        return false;
    }
}