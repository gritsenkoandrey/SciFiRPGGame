using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(StatsManager), typeof(PlayerProgress), typeof(NetworkIdentity))]
public class Player : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Equipment _equipment;
    [SerializeField] private PlayerProgress _progress;

    [SerializeField] private StatsManager _statsManager;

    private NetworkConnection _connection;

    public Character character { get { return _character; } }
    public Inventory inventory { get { return _inventory; } }
    public Equipment equipment { get { return _equipment; } }
    public PlayerProgress progress { get { return _progress; } }
    public NetworkConnection connection
    { 
        get 
        { if (_connection == null)
            {
                _connection = GetComponent<NetworkIdentity>().connectionToClient;
            }

            return _connection;
        }
    }

    public void Setup(Character character, Inventory inventory, Equipment equipment, bool isLocalPlayer)
    {
        _progress = GetComponent<PlayerProgress>();
        _statsManager = GetComponent<StatsManager>();
        _character = character;
        _inventory = inventory;
        _equipment = equipment;
        _character.player = this;
        _inventory.player = this;
        _equipment.player = this;
        _statsManager.player = this;

        if (GetComponent<NetworkIdentity>().isServer)
        {
            UserAccount account = AccountManager.GetAccount(GetComponent<NetworkIdentity>().connectionToClient);
            _character.Stats.Load(account.data);
            _character.unitSkills.Load(account.data);
            _progress.Load(account.data);
            _inventory.Load(account.data);
            _equipment.Load(account.data);
            _character.Stats.manager = _statsManager;
            _progress.manager = _statsManager;
        }

        if (isLocalPlayer)
        {
            InventoryUi.instance.SetInventory(_inventory);
            EquipmentUi.instance.SetEquipment(_equipment);
            StatsUi.Instance.SetManager(_statsManager);
            SkillsPanel.Instance.SetSkills(character.unitSkills);
            SkillTree.Instance.SetCharacter(character);
            SkillTree.Instance.SetManager(_statsManager);

            PlayerChat playerChat = GetComponent<PlayerChat>();
            if (playerChat != null)
            {
                if (GlobalChatChannel.instance != null) playerChat.RegisterChannel(GlobalChatChannel.instance);
                ChatChannel localChannel = _character.GetComponent<ChatChannel>();
                if (localChannel != null) playerChat.RegisterChannel(localChannel);
                ChatUI.Instance.SetPlayerChat(playerChat);
            }
        }
    }
}