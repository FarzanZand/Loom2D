using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CraftSlot : UI_ItemSlot
{

    protected override void Start()
    {
        base.Start();
    }

    public void SetupCraftSlot(ItemData_Equipment _data)
    {
        if (_data == null)
            return;
        
        item.data = _data;

        itemImage.sprite = _data.itemIcon;
        itemText.text = _data.itemName;

        // In case item name is too long, make font smaller to fit the room. 
        if (itemText.text.Length > 12)
            itemText.fontSize = itemText.fontSize * 0.7f;
        else itemText.fontSize = 24;
    }

    public override void OnPointerDown(PointerEventData eventData)          // Click this sets up the window based on the item you clicked
    {
        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }

}
