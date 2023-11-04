using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerItemDrop : ItemDrop
{

    // You grab the list of items currently equipped, and interate through them. If Random hits, you drop the item and add it to list
    // The list is used to unequip the item too

    [Header("Player's drop")]
    [SerializeField] private float chanceToLoseItems;
    [SerializeField] private float chanceToLoseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.Instance;

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();  // We do this because otherwise it crashes as foreach count chances when you drop items
        List<InventoryItem> materialsToLose = new List<InventoryItem>();


        // Remove equipped items
        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if(Random.Range(0, 100) <= chanceToLoseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++) 
        {
                inventory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }

        // Remove materials from stash
        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLoseMaterials)
            {
                DropItem(item.data);
                materialsToLose.Add(item);
            }
        }

        for (int i = 0; i < materialsToLose.Count; i++)
        {
            inventory.RemoveItem(materialsToLose[i].data);
        }

    }

}
