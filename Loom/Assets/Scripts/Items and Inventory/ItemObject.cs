using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    // ItemObject.cs hold the logic for representing the item in the game world and interacting with it
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private void SetupVisuals()
    {
        if (itemData == null) // just incase we forget to put itemdata on item
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName; // Name in inspector of created item. 
    }

    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    public void PickupItem()
    {
        if (!Inventory.Instance.CanAddItem() && itemData.itemType == ItemType.Equipment) // Check that equipment inventory is not full. 
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }

        Inventory.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
