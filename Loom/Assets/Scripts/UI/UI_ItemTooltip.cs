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
    // public RectTransform uiObject; // TEST
    // public bool hasMovedToMouse = false; 

    public void ShowTooltip(ItemData_Equipment item)
    {
        if (item == null)
            return;

        // HERE: To make it be at the mouse position. Do some math. Get mouse position. Spawn at Vector2(Xposition - 0.5 Xwidth, Yposition-0.5Ywidth)
        //if (!hasMovedToMouse)
        //{
        //    Vector3 mouseScreenPosition = Input.mousePosition;
        //    uiObject.position = mouseScreenPosition;
        //    hasMovedToMouse = true;
        //}
        // END HERE: Something above makes it run multiple times, sometimes. 

        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();

        if (itemNameText.text.Length > 15)
            itemNameText.fontSize = itemNameText.fontSize * 0.7f; // In case item name is long and overshoots the window, make text smaller.
        else
            itemNameText.fontSize = defaultNameFontSize;

        gameObject.SetActive(true);
        Debug.Log("Show tooltip"); // TODO: WHY DO YOU RUN 5000 TIMES!?!?!
    }

    public void HideTooltip()
    {
        Debug.Log("Hide Tooltip");
        itemNameText.fontSize = defaultNameFontSize;
        //hasMovedToMouse = false; // TEST
        gameObject.SetActive(false);
    }
}
