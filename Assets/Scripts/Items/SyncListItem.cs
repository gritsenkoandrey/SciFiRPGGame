using UnityEngine.Networking;


public class SyncListItem : SyncList<Item>
{
    // считывание данных при получении
    protected override Item DeserializeItem(NetworkReader reader)
    {
        return ItemBase.GetItem(reader.ReadInt32());
    }

    // запись данных перед отправкой
    protected override void SerializeItem(NetworkWriter writer, Item item)
    {
        writer.Write(ItemBase.GetItemId(item));
    }
}