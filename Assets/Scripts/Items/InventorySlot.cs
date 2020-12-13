using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    public Image icon; // иконка слота
    public Button removeButton; // кнопка удаления предмета из слота
    public Inventory inventory; // инвентарь, с которым работает слот

    private Item _item; // предмет, хранящийся в слоте

    // установка предмета в слот
    public void SetItem(Item newItem)
    {
        _item = newItem;
        icon.sprite = _item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        _item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    // обработчик для нажатия на кнопку удаления предмета
    public void OnRemoveButton()
    {
        // вызываем метод инвентаря, удаляющий предмет из слота
        inventory.RemoveItem(_item);
    }

    // обработчик для нажатия на предмет
    public void UseItem()
    {
        // использование предмета
        if (_item != null)
        {
            inventory.UseItem(_item);
        }
    }
}