using UnityEngine.Networking;


public class Equipment : NetworkBehaviour
{
    public event SyncList<Item>.SyncListChanged onItemChanged;
    public SyncListItem items = new SyncListItem();

    public Player player;

    public override void OnStartLocalPlayer()
    {
        items.Callback += ItemChanged;
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
                items.RemoveAt(i);
                break;
            }
        }
        items.Add(item);
        item.Equip(player);
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
            items.RemoveAt(index);
        }
    }
}