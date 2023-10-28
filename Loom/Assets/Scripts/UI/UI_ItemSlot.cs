using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler // this interface allows mouse for event
{
    // Attached to the itemslot object in the inventory holding the item and image of item.

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem _newItem)
    {
        itemImage.color = Color.white; // Make it not transparent after created

        item = _newItem;
        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanupSlot() // We do this because if we do not, when we equip an item, it still shows in inventory. 
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = ""; // Empty stacksize
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.Instance.EquipItem(item.data);
        }
    }
}
