using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Inventory functionality
    // Inventory.cs Uses a list and dictionary to control which items the player has and holds the core logic of inventory. Attached to Inventory-gameobject
    // InventoryItem.cs hold the logic for the item in the inventory. How many stacks, and which item it is.
    // ItemObject.cs hold the logic for representing the item in the game world and interacting with it
    // ItemData.cs hold specific data for what the item is and does, is a scriptable object created via rightclick
    // UI_ItemSlot.cs carries the image and image text of the item to be placed in the Canvas. Objects created
    // ItemDrop.cs attached to enemies carries a list of dropable items and creates instantiates itemdrops objects in the world randomly from that list
    // ItemEffect.cs is a scriptable object where you can design item effects, which can be attached to ItemData_equipment scriptable object items. 

    // Basic flow: You create ItemData as a scriptable object, ItemObject spawns in game world or received in any other way
    // Calls AddItem() in Inventory.cs instance, this creates a newItem object of InventoryItem, passing the itemData to its constructor
    // This newItem is also then added to the inventoryItems list and inventoryDictionary. 

    // UI_itemSlot, the logic for the inventory visuals
    // Every time you add or remove an item, UpdateSlotUI() passes the itemdata to an ItemSlot for each item in inventory. 
    // ItemSlot is a prefab of a canvas-object already added to the canvas in the scene. Empty until filled. 
    // UI_ItemSlot is on each itemslot-gameobject-prefab attached. It takes the count and image, to update the UI of the inventory, which when empty is transparent.
    // Basically, every time you update inventory, it gets run and uses count to update ui, the gameObject ItemSlot is filled. If empty, it is invisible.

    // The name of the item updates automatically in the gameObject/inspector in the OnValidate() function of ItemObject.cs
    #endregion

    #region Using list and Dictionary
    /*
    Using both a List and a Dictionary to manage a collection of items is a common pattern in certain game development scenarios. 
    The List provides the order and sequence for the items while the Dictionary provides fast look-up times. Here are some reasons for this approach:

    Order: Lists maintain an order, which might be important if the sequence in which items are added matters. 
    This is often relevant in an inventory system, where items appear in the order they were picked up or bought.

    Fast Lookup: Dictionaries allow for quick item lookups based on a key. 
    This is helpful if you want to quickly check if an item exists in the inventory or get details of an item without having to loop through a list. 
    This reduces the time complexity from O(n) in a list to O(1) in a dictionary.

    UI Management: In games, inventory items often need to be displayed in a user interface. 
    Lists are more intuitive when it comes to handling UI elements sequentially, like populating slots in an inventory grid.

    Stacking Items: In some inventory systems, like the one you provided, items can stack. 
    When adding an item, it's faster to check if the item already exists in the inventory using a dictionary, then increase the stack count, 
    rather than looping through the list to find and stack the item.

    Combination: Sometimes, you might want the benefits of both fast lookup and order. 
    In such cases, you use a dictionary for quick checks and operations, and a list to maintain a sequence or order.

    In the provided code, the List and Dictionary are used in tandem for efficient inventory management. When an item is added:
    The List (inventory, stash, or equipment) keeps track of the items in the order they were added.
    The Dictionary (inventoryDictionary, stashDictionary, or equipmentDictionary) allows for fast lookups based on the item data, making operations like checking if an item already exists or updating its stack size more efficient.

    In summary, combining lists and dictionaries in this manner leverages the strengths of both data structures, optimizing for both performance (quick lookups) and functionality (maintaining order).
    */
    #endregion

    #region Equipment
    // Equipment is a subtype/child of ItemData called ItemData_Equipment. Main difference is that it has an enum with Weapon, Arnmor, Amulet and Flask, with its own inventory.
    // You have a list+dictionary for it like the rest. 
    // With EquipItem(ItemData _item) you replace current equipment unless empty, you just fill. You then add item to equipment dict/list and remove from inventory. 
    // By the end, you update UI with UpdateSlotUI(); First iterates through to match equipment with the matching enum Ui_EquipmentSlot, then replaces it with data.
    // For its UI, you have an aray of UI_Equipmentslot[] which is a child of UI_ItemSlot, main difference is that it maps to equipment enum-type. 
    // Drag the UI parent that holds all the Slot-prefabs for the equipments to equipmenSlotParent, which shows the visual from UI_EquipmentSlot[].

    // This is also written in ItemData_Equipment.cs, regarding stats on equipments. 
    // Every equipment has an equipment type, and when you equip an item from inventory, it is removed from the inventory
    // and placed in the equipment slot, while removing the old equipment and placing that in inventory. Check inventory.cs for more info.
    // Equipment has stats. in Inventory.cs, AddModifiers() is called when you equip, RemoveModifiers() is called when you unequip.

    // Equipment stats
    // We reach stats via PlayerStats playerStats, which is a child of the parent CharacterStats, holding all different stat-objects. 
    // Every stat its own object and reached via playerStats.statName. Defined in CharacterStats.cs, which created object per stat based from the Stat.cs class. 
    // When you add a modifier of a stat with AddModifiers(), you populate the List<int> modifiers in stat.cs of that stat. 
    // The final value is calculated using GetValue() in Stat.cs. It takes base value, and adds all modifiers to it. 
    // RemoveModifier() does the reverse, just removes it from the list. 

    // Unequiping equipment
    // When you click on the itemslot-prefab holding the UI_EquipmentSlot.cs script, it runs Inventory.instance.UnequipItem() and AddItem(), then cleanups UI.
    // UnequipItem() checks if item is in equipmentdictionary. If so, remove it with Remove(), which removes it from the list and dictionary, hence unequipped. 
    // It then takes same item and runs AddItem(), where it adds the item to inventory via AddToInventory(_item) which either ups stack, and if null, adds item to inventory
    #endregion

    #region Materials and crafting
    // Crafting uses materials, which are stored in the stash and stashDictionary. 
    // Every Equipment created (scriptable object) has crafting materials that can be defined in the list inspector which you use to setup a craftable item
    // In the ItemSlot object with UI_CraftSlot attached to it, select the craftable equipment item in the "Data" field, which will register the materials defined. 
    // When you click on the craftable item icon, UI_CraftSlot.cs runs CanCraft(craftData, craftingMaterials) here in inventory.cs 
    // craftData is the item to craft, and craftingMaterials is the materials needed to craft it. 
    // It checks if material is in stash, if so, remove the material from stash and create item. If not, do not create item. 
    #endregion

    #region Item effect
    // ItemEffect.cs is a scriptable object where you can create a data object holding an item effect to attach to unique items. 
    // The effects themselves will be a new .cs script each, which inherits from ItemEffect.cs. Also need to update the path-name for creating it with right click.
    // Example: [CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item effect/Thunder strike")]
    // This data object can be attached to an equipment. Each ItemData_equipment.cs holds ItemEffect[] itemEffects variable. 
    // When you create a new equipment scriptable-object, you will therefore have a itemEffects array you can populate in inspector, filling up ItemEffect[]
    // ItemData_equipment.cs has a function called Effect() which will run all item effects attached by calling item.ExecuteEffect() for each item in ItemEffect[]
    // This function can be called from wherever in the game. For instance, on attack with a weapon, or using a potion. 
    // You can refer to the item as example with ItemData_Equipment equippedAmulet = Inventory.Instance.GetEquipment(EquipmentType.Amulet);
    #endregion

    #region Use Flask
    // You have one flask equipped and can reach it with ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);
    // In this Inventory.cs script, you have the function UseFlask(), this function calls all effects on the flask with currentFlask.Effect(null);
    #endregion

    public static Inventory Instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]

    [SerializeField] private Transform inventorySlotParent;                         // Drag the inventory parent from the canvas here, the one holding all the UI objects
    [SerializeField] private Transform stashSlotParent;                             // Drag the stash parent from the canvas here, the one holding all the UI objects
    [SerializeField] private Transform equipmentSlotParent;                         // Drag the equipment parent from the canvas here, the one holding all the UI objects

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;

    [Header("Items cooldown")]
    private float lastTimeUsedFlask; 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();                                  // list of item objects to present in inventory
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();        // List of items owned by player


        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();  // Will fill the array with the children of the component inventorySlotParent
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        
        AddStartingItems();
    }

    private void AddStartingItems()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;          // Convert ItemData to ItemData_Equipment so we can add it to the dictionary. 
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;


        // Do these steps to replace already equipped item of same equipment type. New item is removed from inventory when equipped, and old item is replaced there
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)           // If you equip something that is already equipped, i.e. a sword while wearing one, delete the existing one. 
                oldEquipment = item.Key;                                        // Assigned to the item already in the dictionary
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);                                          // Unequip old equipment
            AddItem(oldEquipment);                                              // add old equipment back to inventory
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();
        RemoveItem(_item);

        UpdateSlotUI(); 
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)                            // Check that the type matches the slot it iterates through. i.e. Armor == armor slot.  
                    equipmentSlot[i].UpdateSlot(item.Value);                                        // Update the slot with the matching equipment type. 
            }
        }
        
        for (int i = 0; i < inventoryItemSlot.Length; i++)  // We do this because if we do not, when we equip an item, it still shows in inventory. 
        {
            inventoryItemSlot[i].CleanupSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanupSlot();
        }



        for (int i = 0; i < inventory.Count; i++)                               // Creates itemslots as many as you have items in inventory
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
    }

    public void AddItem(ItemData _item)                                         // If you already have the item, just add a stack to it
    {
        if (_item.itemType == ItemType.Equipment)
            AddToInventory(_item);

        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);

        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))    // If item already in inventory, just add a stack
        {
            value.AddStack();
        }
        else                                                                    // If you don't have it. Add it to your inventory and inventoryDictionary
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))    // If item already in inventory, just add a stack
        {
            value.AddStack();
        }
        else                                                                    // If you don't have it. Add it to your inventory and inventoryDictionary
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))    // Check if item to remove is in inventory. 
        {
            if (value.stackSize <= 1)                                           // If you have one item, remove item completely, if more than one, remove a stack only
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
                stashValue.RemoveStack();
        }

        UpdateSlotUI();
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();                              // Temp list to hold material items to remove

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue)) // See if material is in stash, and add it to list if so
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("Not enough materials");               
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);              // Add the material to the list, which will be used to empty stash when item is created
                }
            }
            else
            {
                Debug.Log("Not Enough materials");
                return false; 
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)           // Remove the materials from the craft list using the populated list
        {
            RemoveItem(materialsToRemove[i].data);
        }

        AddItem(_itemToCraft);                                      // Success! If you make it here, you got your new item.
        Debug.Log("Here is your item " + _itemToCraft.name);
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;                 // Return a list of all equipment on player
    public List<InventoryItem> GetStashList() => stash;                         // Return a list of all stash on player

    public ItemData_Equipment GetEquipment(EquipmentType _type)                 // Get access to any item equipped on player
    {
        ItemData_Equipment equippedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)           
                equippedItem = item.Key;                                        
        }

        return equippedItem;
    }
    public void UseFlask()
    {
        Debug.Log("BEFORE NULL");
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
            return;

        bool canUseFlask = Time.time > lastTimeUsedFlask + currentFlask.itemCooldown;

        if (canUseFlask)
        {
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
            Debug.Log("Flask on cooldown");
    }
}
