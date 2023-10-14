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

    public void OnPointerDown(PointerEventData eventData)
    {
        if(item.data.itemType == ItemType.Equipment)
        Debug.Log("Equipped new item + " + item.data.itemName);
    }
}
