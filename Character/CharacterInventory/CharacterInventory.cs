using System;
using System.Collections.Generic;
using HGS.LastBastion.Items;
using UnityEngine;

namespace HGS.LastBastion.Character.Inventory
{
    public class CharacterInventory : MonoBehaviour
    {
        [SerializeField] private List<Item> resourcesInventory = new List<Item>();

        public event Action<Item> OnItemAdd;
        public event Action<Item> OnNewItemAdd;

        public void AddItem(Item item)
        {
            if (ShouldStackItem(item.itemName))
            {
                foreach (var _item in resourcesInventory)
                {
                    if (_item.itemName == item.itemName)
                    {
                        _item.itemQuantity += item.itemQuantity;
                        OnItemAdd?.Invoke(_item);
                    }
                }
                return;
            }
            resourcesInventory.Add(item);
            item.itemQuantity++;
            OnNewItemAdd?.Invoke(item);
        }

        private bool ShouldStackItem(string _itemName)
        {
            foreach (var item in resourcesInventory)
            {
                if (item.itemName == _itemName) return true;
            }

            return false;
        }

        public bool UseItem(Item item, int ammount)
        {
            foreach (var _item in resourcesInventory)
            {
                if (item == _item)
                {
                    if (item.itemQuantity >= ammount)
                    {
                        item.itemQuantity -= ammount;
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        public bool UseItem(string itemName, int amount)
        {
            foreach (var item in resourcesInventory)
            {
                if (item.itemName == itemName)
                {
                    if (item.itemQuantity >= amount)
                    {
                        item.itemQuantity -= amount;
                        return true;
                    }
                    else
                    {
                        return false; // Not enough quantity
                    }
                }
            }

            return false; // Item not found
        }



    }
}
