using UnityEngine;
using UnityEngine.Networking;


public class Inventory : NetworkBehaviour
{
    public Transform dropPoint;
    public int space = 20;
    public event SyncList<Item>.SyncListChanged onItemChanged;

    public SyncListItem items = new SyncListItem();

    public override void OnStartLocalPlayer()
    {
        items.Callback += ItemChanged;
    }

    private void ItemChanged(SyncList<Item>.Operation op, int itemIndex)
    {
        onItemChanged(op, itemIndex);
    }

    public bool Add(Item item)
    {
        if (items.Count < space)
        {
            items.Add(item);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Remove(Item item)
    {
        CmdRemoveItem(items.IndexOf(item));
    }

    [Command]
    private void CmdRemoveItem(int index)
    {
        if (items[index] != null)
        {
            Drop(items[index]);
            items.RemoveAt(index);
        }
    }

    private void Drop(Item item)
    {
        ItemPickup pickupItem = Instantiate(item.pickupPrefab, dropPoint.position, Quaternion.Euler(0f, Random.Range(0f, 360.0f), 0f));
        pickupItem.item = item;
        NetworkServer.Spawn(pickupItem.gameObject);
    }
}