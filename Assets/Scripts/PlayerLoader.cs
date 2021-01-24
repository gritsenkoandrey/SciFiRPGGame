using UnityEngine;
using UnityEngine.Networking;


public class PlayerLoader : NetworkBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Player _player;

    public override void OnStartAuthority()
    {
        CmdCreatePlayer();
    }

    // [Command] говорит нам о том, что это метод нужно выполнить на сервере
    // Cmd в названии метода обязательно, иначе не скомпилируется
    [Command]
    public void CmdCreatePlayer()
    {
        Character character = CreateCharacter();
        _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), isLocalPlayer);
        _playerController.SetCharacter(character, isLocalPlayer);
    }

    public Character CreateCharacter()
    {
        // создаём персонажа по хешу из пользовательский данных
        UserAccount account = AccountManager.GetAccount(connectionToClient);
        GameObject unitPrefab = NetworkManager.singleton.spawnPrefabs.Find(x => x.GetComponent<NetworkIdentity>().assetId.Equals(account.data.characterHash));
        GameObject unit = Instantiate(unitPrefab, account.data.posCharacter, Quaternion.identity);
        // указываем объект игрока для определения видимости персонажа
        Character character = unit.GetComponent<Character>();
        character.player = _player;
        // реплицируем персонажа
        NetworkServer.Spawn(unit);
        // настраиваем персонажа на клиенте которому он пренадлежит
        TargetLinkCharacter(connectionToClient, unit.GetComponent<NetworkIdentity>());
        return character;
    }

    [TargetRpc]
    private void TargetLinkCharacter(NetworkConnection target, NetworkIdentity unit)
    {
        Character character = unit.GetComponent<Character>();
        _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), true);
        _playerController.SetCharacter(character, true);
    }

    public override bool OnCheckObserver(NetworkConnection connection)
    {
        return false;
    }

    private void OnDestroy()
    {
        if (isServer && _player.character != null)
        {
            UserAccount account = AccountManager.GetAccount(connectionToClient);
            account.data.posCharacter = _player.character.transform.position;
            Destroy(_player.character.gameObject);
            NetworkManager.singleton.StartCoroutine(account.Quit());
        }
    }
}