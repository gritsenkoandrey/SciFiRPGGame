using UnityEngine;


public class ItemPickup : Interactable
{
    public Item item;
    public float lifetime;

    private void Update()
    {
        if (isServer)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public override bool Interact(GameObject user)
    {
        return PickUp(user);
    }

    public bool PickUp(GameObject user)
    {
        Character character = user.GetComponent<Character>();
        if (character != null && character.player.inventory.AddItem(item))
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