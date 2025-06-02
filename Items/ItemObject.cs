using HGS.LastBastion.Character.Inventory;
using UnityEngine;

namespace HGS.LastBastion.Items
{
    public class ItemObject : MonoBehaviour
    {
        public ItemSO itemSO;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            CharacterInventory targetInventory = other.gameObject.GetComponent<CharacterInventory>();

            if (targetInventory == null) return;

            Item itemToAdd = new Item(itemSO.itemName);
            itemToAdd.itemQuantity = Random.Range(10, 26);

            targetInventory.AddItem(itemToAdd);

            Destroy(gameObject);
        }
    }
}
