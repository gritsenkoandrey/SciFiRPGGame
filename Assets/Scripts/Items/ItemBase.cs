using UnityEngine;


public class ItemBase : MonoBehaviour
{
    public static ItemCollection itemCollection;
    [SerializeField] private ItemCollection _itemCollection;

    private void Awake()
    {
        if (itemCollection != null)
        {
            if (_itemCollection != itemCollection)
            {
                Debug.LogError("More than one ItemCollection found!");
                return;
            }
        }
        itemCollection = _itemCollection;
    }

    public static int GetItemId(Item item)
    {
        for (int i = 0; i < itemCollection.items.Length; i++)
        {
            if (item == itemCollection.items[i])
            {
                return i;
            }
        }

        if (item != null)
        {
            Debug.LogError("Items " + item.name + " not found in ItemBase!");
        }

        return -1;
    }

    public static Item GetItem(int id)
    {
        return id == -1 ? null : itemCollection.items[id];
    }
}