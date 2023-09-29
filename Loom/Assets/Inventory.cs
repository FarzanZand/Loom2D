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

    // Basic flow: You create ItemData as a scriptable object, ItemObject spawns in game world or received in any other way
    // Calls AddItem() in Inventory.cs instance, this creates a newItem object of InventoryItem, passing the itemData to its constructor
    // This newItem is also then added to the inventoryItems list and inventoryDictionary. 

    // Why use dictionary and not just InventoryItem?
    #endregion


    public static Inventory Instance;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();                             // list of item objects to present in inventory
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();        // List of items owned by player
    }

    public void AddItem(ItemData _item)                                         // If you already have the item, just add a stack to it
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))    // If item already in inventory, just add a stack
        {
            value.AddStack();
        }
        else                                                                    // If you don't have it. Add it to your inventory and inventoryDictionary
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)                                  
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))    // Check if item to remove is in inventory. 
        {
            if (value.stackSize <= 1)                                           // If you have one item, remove item completely, if more than one, remove a stack only
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();
        }
    }
}
