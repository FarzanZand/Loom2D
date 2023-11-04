using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour

    // ItemDrop.cs is attached to the enemy gameobject.
    // An ItemDrop object is created in EnemyStats.cs as myDropSystem, and on enemy Die(), myDropSystem.GenerateDrop() is called. 
    // GenerateDrop will check how many items are in the possibleDrop[], and do a drop-roll for each. Chance % is defined in the item data scriptable object.
    // Every successful roll is added the the droplist. You then pick items from the list = to the value of possibleItemDrops.  They are then instantiated / dropped in DropItem()

{
    [SerializeField] private int possibleItemDrops;                         // How many items enemy can drop
    [SerializeField] private ItemData[] possibleDrop;                       // Attach item objects in the inspector to fill array, list of possible drops
    private List<ItemData> dropList = new List<ItemData>();                 // Will be populated when GenerateDrop() runs, holds final list of items to drop

    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)                       // Generate the droplist of all items passing the dropchance
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
                dropList.Add(possibleDrop[i]);
        }

        for (int i = 0; i < possibleItemDrops; i++)                         // If you want to drop only one or more items, select a random item from list per amountOfItems
        {

            if (dropList.Count == 0) // This is added from comment, not teacher. A bug he will fix at end of course. This return thing.
            {
                return;
            }

            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];

            dropList.Remove(randomItem);
            DropItem(randomItem);                                           // Pass the itemData to DropItem() which instantiates it
        }
    }

    protected void DropItem(ItemData _itemData)                                 // Create a newDrop GameObject, Setup this object with the itemdata of the item passed to it   
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity); // DroPrefab is an unpopulated dropObject that can take itemData (the scriptable object created). 

        Vector2 randomVelocity = new Vector2(Random.Range(-5,5), Random.Range(15,20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
