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
    // Attached the Craftlistbutton - holder child objects SetupCraftList - EquipmentType.
    // It is the equipment type icons to the left. You click them, they show a list of craftable items of that type in the ScrollView.
    // You populate a list of craftable items by filling the craftEquipment<ItemData_Equipment> list in the inspector.
    // In its SetupCraftList(), it takes that list, and instantiates a prefab using the craftSlotPrefab for each. Called on click. 
    // Sets up each new slot with newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
    #endregion

    #region Scrollview: holding the list of craftable items from that equipment type
    // UI_CraftList.craftEquipment is a list of equipment scriptable data objects which is dragged into the inspector in CraftListButton-objects
    // These objects (one per equipment type) when clicked runs UI_CraftList.cs SetupCraftList() 
    // Clicking that, instantiates a prefab of UI_CraftSlot per dataobject and adds it to the list.
    #endregion

    #region CraftWindow: holding the information for the item being crafted, and the craft button. 
    // Attached to CraftWindow gameObject in the Craft_UI gameObject.
    // This section holds the information on the craftable item. Stats, materials needed, craft button.
    // SetupCraftWindow() starts by getting the materials needed for that item. 
    // It is called when you click on the CraftSlot icon button in the scrollview, selecting the item you want to see craft info on. 
    // It checks the crafting material needed in the craftinMaterials<InventoryItem> list in ItemData_Equipment for the crafting materials
    #endregion

    #region Stash: holding all the materials you can use for crafting
    // it's the Inventory.stashSlotParent object holding the reference. Inventory.cs is attached to the gameObject inventory. 
    // In Inventory.cs, there is UI_ItemSlot[] stashItemSlot which hold a reference to alla material slots to populate. 
    // In Inventory.cs, stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>() populates the array with all gameObjects in children Stash with UI_ItemSlot scripts. 
    // Now that stashItemSlot is filled with empty packages, it needs to get populated which happens through Inventory.UpdateSlotUI()
    // stashItemSlot[i].UpdateSlot(stash[i]) takes all items in the stash and updates enough stash slots until no more materials left, matches due to .length of stash. 
    // In other words, Got the logic written down for how stash works.
    // It basically takes a reference to the stash game object, populates an array for each object there. Then in UpdateSlotUI(), fills them for each item in stash dictionary list.
    #endregion


    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImage;                     // Drag the 4 MaterialIcon from the parent gameobject MaterialList. These are the required material to craft the item

    public void SetupCraftWindow(ItemData_Equipment _data)              // Setup the materials in the CraftWindow list of required materials to craft
    {
        // Clear it from old data
        craftButton.onClick.RemoveAllListeners();                       // Just in case, listener added later down below. 
        for (int i = 0; i < materialImage.Length; i++)                  // each of the 1-4 materialslots, dragged in from inspector. 
        {
            materialImage[i].color = Color.clear;                       // Clear the window, making all four windows and text transparent/hidden for setup.
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        // Fill it with new data
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
