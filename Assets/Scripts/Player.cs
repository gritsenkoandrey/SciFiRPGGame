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

    public Character character { get { return _character; } }
    public Inventory inventory { get { return _inventory; } }
    public Equipment equipment { get { return _equipment; } }
    public PlayerProgress progress { get { return _progress; } }

    public void Setup(Character character, Inventory inventory, Equipment equipment, bool isLocalPlayer)
    {
        _statsManager = GetComponent<StatsManager>();
        _progress = GetComponent<PlayerProgress>();
        _character = character;
        _inventory = inventory;
        _equipment = equipment;
        _character.player = this;
        _inventory.player = this;
        _equipment.player = this;
        _statsManager.player = this;

        if (GetComponent<NetworkIdentity>().isServer)
        {
            _character.stats.manager = _statsManager;
            _progress.manager = _statsManager;
        }

        if (isLocalPlayer)
        {
            InventoryUi.instance.SetInventory(_inventory);
            EquipmentUi.instance.SetEquipment(_equipment);
            StatsUi.Instance.SetManager(_statsManager);
        }
    }
}