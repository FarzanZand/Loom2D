using System;

[Serializable]
public class InventoryItem
{
    // ItemObject.cs when received, calls Inventory.Instance.AddItem(itemData)
    // AddItem() from Inventory.cs creates an InventoryItem, and passes along its itemData to the constructor here

    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData _newItemData) // Constructor
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack()
    {
        stackSize++;
    }
    public void RemoveStack()
    {
        stackSize--;
    }

}
