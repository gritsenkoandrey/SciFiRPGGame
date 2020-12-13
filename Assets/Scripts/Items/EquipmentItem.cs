using UnityEngine;


[CreateAssetMenu(fileName = "New equipment", menuName = "Inventory/Equipment")]
public class EquipmentItem : Item
{
    public EquipmentSlotType equipSlot;

    public int damageModifier;
    public int armorModifier;
    public int speedModifier;

    public override void Use(Player player)
    {
        player.inventory.RemoveItem(this);
        EquipmentItem oldItem = player.equipment.EquipItem(this);
        if (oldItem != null)
        {
            player.inventory.AddItem(oldItem);
        }
        base.Use(player);
    }
}