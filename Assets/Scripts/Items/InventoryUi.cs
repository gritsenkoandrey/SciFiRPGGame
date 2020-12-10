using UnityEngine;


public class InventoryUi : MonoBehaviour
{
    public static InventoryUi instance;

    [SerializeField] private GameObject _inventoryUi;
    [SerializeField] private Transform _itemsParent; // объект, в котором должны храниться слоты
    [SerializeField] private InventorySlot _slotPrefab; // массив слотов для текущего инвентаря

    private InventorySlot[] _slots;
    private Inventory _inventory;

    private void Awake()
    {
        _inventoryUi.SetActive(false);
        if (instance != null)
        {
            Debug.LogError("More than one instance of InventoryUI found!");
            return;
        }
        instance = this;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            _inventoryUi.SetActive(!_inventoryUi.activeSelf);
        }
    }

    public void SetInventory(Inventory newInventory)
    {
        _inventory = newInventory;
        _inventory.onItemChanged += ItemChanged;
        InventorySlot[] childs = _itemsParent.GetComponentsInChildren<InventorySlot>(); // получение старых слотов
        // удаление старых слотов
        for (int i = 0; i < childs.Length; i++)
        {
            Destroy(childs[i].gameObject);
        }
        _slots = new InventorySlot[_inventory.space];
        for (int i = 0; i < _inventory.space; i++)
        {
            _slots[i] = Instantiate(_slotPrefab, _itemsParent); // создание нового слота
            _slots[i].inventory = _inventory; // настройка слота в соответствии со списком предметов из инвентаря
            if (i < _inventory.items.Count)
            {
                _slots[i].SetItem(_inventory.items[i]);
            }
            else
            {
                _slots[i].ClearSlot();
            }
        }
    }
    private void ItemChanged(UnityEngine.Networking.SyncList<Item>.Operation op, int itemIndex)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < _inventory.items.Count) _slots[i].SetItem(_inventory.items[i]);
            else _slots[i].ClearSlot();
        }
    }
}