using UnityEngine;

namespace HGS.LastBastion.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
    [System.Serializable]
    public class ItemSO : ScriptableObject
    {
        [Header("Item Name")]
        public string itemName;

        [Space]
        [Header("Item Description")]
        [TextArea(3, 10)]
        public string itemDescription;

        [Space]
        [Header("Item Quantity")]
        public int itemQuantity;
        public int maximumQuantity;

        [Space]
        [Header("Item Condition")]
        public bool consumable = false;
    }
}
