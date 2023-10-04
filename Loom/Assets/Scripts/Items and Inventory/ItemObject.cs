using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    // ItemObject.cs hold the logic for representing the item in the game world and interacting with it
    
    [SerializeField] private ItemData itemData;

    private void OnValidate() // Automatically updates in the inspector even when not playing. 
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName; // Name in inspector of created item. 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent <Player> () != null)
        {
            Inventory.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
