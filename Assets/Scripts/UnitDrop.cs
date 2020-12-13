using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(Unit))]
public class UnitDrop : NetworkBehaviour
{
    [SerializeField] DropItem[] dropItems = new DropItem[0];

    public override void OnStartServer()
    {
        GetComponent<Unit>().EventOnDie += Drop;
    }

    private void Drop()
    {
        for (int i = 0; i < dropItems.Length; i++)
        {
            if (Random.Range(0, 100f) <= dropItems[i].rate)
            {
                ItemPickup pickupItem = Instantiate(dropItems[i].item.pickupPrefab, transform.position, Quaternion.Euler(0, Random.Range(0, 360f), 0));
                pickupItem.item = dropItems[i].item;
                NetworkServer.Spawn(pickupItem.gameObject);
            }
        }
    }

    [System.Serializable]
    struct DropItem
    {
        public Item item;
        [Range(0, 100f)]
        public float rate;
    }
}