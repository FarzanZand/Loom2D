using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler // These interfaces gives events on pointer actions
{
    // Attached to the itemslot object in the inventory holding the item and image of item.

    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        itemImage.color = Color.white; // Make it not transparent after created

        item = _newItem;
        if (item != null)
        {
            itemImage.sprite = item.data.itemIcon;
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

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) 
            return;

        if (Input.GetKey(KeyCode.LeftControl)) // If you just want to delete an item from inventory.
        {
            Inventory.Instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.Instance.EquipItem(item.data);
        }

        ui.itemTooltip.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null) 
            return;

        Vector2 mousePosition = Input.mousePosition;

        // Place the Tooltip near the mouse
        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)                                            // Mouse is on the right side of screen
            xOffset = -200;
        else
            xOffset = 200;

        if (mousePosition.y > 320)
            yOffset = -200;
        else
            yOffset = 200;

        ui.itemTooltip.ShowTooltip(item.data as ItemData_Equipment);
        ui.itemTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;

        ui.itemTooltip.HideTooltip();
    }
}
