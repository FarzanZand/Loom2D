using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

#region Core functionality
// This script is attached to the canvas gameobject Item_Tooltip.
// The gameobject Item_Tooltip is also attached to the UI.cs script in Canvas via inspector
// in UI_ItemSlot, you have mouseenter and mouseexit interface attached. YOu also have a reference to the UI.cs script ui = GetComponentInParent<UI>
// When entering an itemslot with UI_ItemSlot.cs attached, or exiting, they run the ShowTooltip() and HideTooltip() functions.
// They do this by by calling ui.ItemTooltip.ShowTooltip() in UI_ItemSlot.cs to reach this .cs file. 
// Same logic is used to show the tooltip for stats.
#endregion

#region Tooltip for equipment text
// Populates the name, type and description.text variables in ItemData_Equipment. Gets description via item.GetDescription();
// GetDescription() gets every value from a stat modifier on an item. If it is more than 0, you append it to the description text.
#endregion

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private int defaultNameFontSize = 32;

    public void ShowTooltip(ItemData_Equipment item)
    {
        if (item == null)
            return;

        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();

        if (itemNameText.text.Length > 15)
            itemNameText.fontSize = itemNameText.fontSize * 0.7f; // In case item name is long and overshoots the window, make text smaller.
        else
            itemNameText.fontSize = defaultNameFontSize;

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        itemNameText.fontSize = defaultNameFontSize;
        //hasMovedToMouse = false; // TEST
        gameObject.SetActive(false);
    }
}
