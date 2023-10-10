using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Inventory functionality
    // Main Classes
    // Inventory.cs Uses a list and dictionary to control which items the player has
    // InventoryItem.cs hold the logic for the item in the inventory. How many stacks, and which item it is
    // ItemObject.cs hold the logic for representing the item in the game world and interacting with it
    // ItemData.cs hold specific data for what the item is and does, is a scriptable object
    // UI_ItemSlot.cs carries the image and image text of the item to be placed in the Canvas. Objects created

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


    public static Inventory Instance;

    public List<InventoryItem> inventory; 
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary <ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]

    [SerializeField] private Transform inventorySlotParent;                     // Drag the inventory parent from the canvas here, the one holding all the UI objects
    [SerializeField] private Transform stashSlotParent;                         // Drag the stash parent from the canvas here, the one holding all the UI objects

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
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

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();  // Will fill the array with the children of the component inventorySlotParent
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }
    private void UpdateSlotUI()
    {
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

        else if(_item.itemType == ItemType.Material)
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

        if(stashDictionary.TryGetValue(_item,out InventoryItem stashValue))
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
}
