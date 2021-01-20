using UnityEngine.Networking;


public class Equipment : NetworkBehaviour
{
    public event SyncList<Item>.SyncListChanged onItemChanged;
    public SyncListItem items = new SyncListItem();

    public Player player;
    private UserData _data;

    public override void OnStartLocalPlayer()
    {
        items.Callback += ItemChanged;
    }

    public void Load(UserData data)
    {
        _data = data;
        for (int i = 0; i < data.equipment.Count; i++)
        {
            EquipmentItem item = (EquipmentItem)ItemBase.GetItem(data.equipment[i]);
            items.Add(item);
            item.Equip(player);
        }
    }

    private void ItemChanged(SyncList<Item>.Operation op, int itemIndex)
    {
        onItemChanged(op, itemIndex);
    }

    public EquipmentItem EquipItem(EquipmentItem item)
    {
        EquipmentItem oldItem = null;
        for (int i = 0; i < items.Count; i++)
        {
            if (((EquipmentItem)items[i]).equipSlot == item.equipSlot)
            {
                oldItem = (EquipmentItem)items[i];
                oldItem.Unequip(player);
                _data.equipment.Remove(ItemBase.GetItemId(items[i]));
                items.RemoveAt(i);
                break;
            }
        }
        items.Add(item);
        item.Equip(player);
        _data.equipment.Add(ItemBase.GetItemId(item));

        return oldItem;
    }

    public void UnequipItem(Item item)
    {
        CmdUnequipItem(items.IndexOf(item));
    }

    [Command]
    private void CmdUnequipItem(int index)
    {
        if (items[index] != null && player.inventory.AddItem(items[index]))
        {
            ((EquipmentItem)items[index]).Unequip(player);
            _data.equipment.Remove(ItemBase.GetItemId(items[index]));
            items.RemoveAt(index);
        }
    }
}