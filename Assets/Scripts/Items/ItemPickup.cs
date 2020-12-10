using UnityEngine;


public class ItemPickup : Interactable
{
    public Item item;

    public override bool Interact(GameObject user)
    {
        return PickUp(user);
    }

    public bool PickUp(GameObject user)
    {
        Character character = user.GetComponent<Character>();
        if (character != null && character.inventory.Add(item))
        {
            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }
}