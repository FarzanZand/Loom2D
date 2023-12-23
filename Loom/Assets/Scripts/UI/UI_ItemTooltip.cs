using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// This script is attached to the canvas gameobject Item_Tooltip.
// The gameobject Item_Tooltip is also attached to the UI.cs script in Canvas via inspector
// in UI_ItemSlot, you have mouseenter and mouseexit interface attached. YOu also have a reference to the UI.cs script ui = GetComponentInParent<UI>
// When entering an itemslot with UI_ItemSlot.cs attached, or exiting, they run the ShowTooltip() and HideTooltip() functions.
// They do this by by calling ui.ItemTooltip.ShowTooltip() in UI_ItemSlot.cs to reach this .cs file. 

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    void Start()
    {
        
    }

    public void ShowTooltip(ItemData_Equipment item)
    {
        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();

        gameObject.SetActive(true);
    }

    public void HideTooltip() => gameObject.SetActive(false);

}
