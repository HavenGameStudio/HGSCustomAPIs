using System;
using UnityEngine;

namespace HGS.LastBastion.Items
{
    [Serializable]
    public class Item
    {
        public string itemName;
        public int itemQuantity;

        public Item(string _itemName)
        {
            itemName = _itemName;
        }
    }
}
