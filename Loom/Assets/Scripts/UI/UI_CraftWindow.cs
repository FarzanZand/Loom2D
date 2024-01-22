using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    // Craft_UI is divided into Four sections. CraftList, Scrollview, CraftWindow and Stash
    // See UI_CraftWindow.cs for documentation of the crafting window.

    #region CraftList: holding the equipment types to the left
    // asd
    #endregion

    #region Scrollview: holding the list of craftable items from that equipment type
    // asd
    #endregion

    #region CraftWindow: holding the information for the item being crafted, and the craft button. 
    // asd
    #endregion

    #region Stash: holding all the materials you can use for crafting
    // asd
    #endregion


    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImage;                     // Drag the 4 MaterialIcon from the parent gameobject MaterialList

    public void SetupCraftWindow(ItemData_Equipment _data)              // Setup the materials in the list
    {
        craftButton.onClick.RemoveAllListeners();                       // Just in case, listener added later down below. 

        for (int i = 0; i < materialImage.Length; i++)                  // each of the 1-4 materialslots, dragged in from inspector. 
        {
            materialImage[i].color = Color.clear;                       // Clear the window, making all four windows and text transparent/hidden for setup.
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < _data.craftingMaterials.Count; i++)         // Get the amount of different types of craftingMaterials needed. 1-4
        {
            if (_data.craftingMaterials.Count > materialImage.Length)   // If more than 4, error message
                Debug.LogWarning("You have more material amount than you have material slots in craft window. 4 max.");

            // Per iteration, fill icon data
            materialImage[i].sprite = _data.craftingMaterials[i].data.itemIcon; // Add image icon
            materialImage[i].color = Color.white;                               // Make it visible

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.text = _data.craftingMaterials[i].stackSize.ToString();    // Get the stack size needed to craft
            materialSlotText.color = Color.white;                                       // Make it visible
        }


        itemIcon.sprite = _data.itemIcon;                               // Set up icon
        itemName.text = _data.name;
        itemDescription.text = _data.GetDescription();

        // Add listener to craft button, being the item data method of that item, so it reacts when clicked and nothing else.
        craftButton.onClick.AddListener(() => Inventory.Instance.CanCraft(_data, _data.craftingMaterials));
    }
}
