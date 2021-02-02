using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentUi : MonoBehaviour
{
    public static EquipmentUi instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of InventoryUI found!");
            return;
        }
        instance = this;
    }

    [SerializeField] GameObject equipmentUI;
    [Space]
    [SerializeField] EquipmentSlot headSlot;
    [SerializeField] EquipmentSlot chestSlot;
    [SerializeField] EquipmentSlot legsSlot;
    [SerializeField] EquipmentSlot righHandSlot;
    [SerializeField] EquipmentSlot leftHandSlot;

    Equipment equipment;
    EquipmentSlot[] slots;

    private void Start()
    {
        equipmentUI.SetActive(false);
        slots = new EquipmentSlot[System.Enum.GetValues(typeof(EquipmentSlotType)).Length];
        slots[(int)EquipmentSlotType.Chest] = chestSlot;
        slots[(int)EquipmentSlotType.Head] = headSlot;
        slots[(int)EquipmentSlotType.LeftHand] = leftHandSlot;
        slots[(int)EquipmentSlotType.Legs] = legsSlot;
        slots[(int)EquipmentSlotType.RighHand] = righHandSlot;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Equipment") && EventSystem.current.currentSelectedGameObject == null)
        {
            equipmentUI.SetActive(!equipmentUI.activeSelf);
        }
    }

    public void SetEquipment(Equipment newEquipment)
    {
        equipment = newEquipment;
        equipment.onItemChanged += ItemChanged;
        for (int i = 0; i < slots.Length; i++) if (slots[i] != null) slots[i].equipment = equipment;
        ItemChanged(0, 0);
    }

    private void ItemChanged(UnityEngine.Networking.SyncList<Item>.Operation op, int itemIndex)
    {
        for (int i = 0; i < slots.Length; i++) slots[i].ClearSlot();
        for (int i = 0; i < equipment.items.Count; i++)
        {
            slots[(int)((EquipmentItem)equipment.items[i]).equipSlot].SetItem(equipment.items[i]);
        }
    }
}