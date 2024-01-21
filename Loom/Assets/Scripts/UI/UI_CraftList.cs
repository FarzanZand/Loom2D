using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    // The middle window, showing a list of all craftable items. If you click one, it updates the UI_CraftWindow

    [SerializeField] private Transform craftSlotParent;             // The CraftSlotsParent gameobject in the canvas craft viewport. Drag in inspector
    [SerializeField] private GameObject craftSlotPrefab;            // CraftSlotPrefab created, drag in from assets

    [SerializeField] private List<ItemData_Equipment> craftEquipment;
    void Start()
    {
        // Get the first child of the parent CraftListButton - Holder, which is the game object SetupCraftList - Weapon.
        // Set up the default list. 
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        SetupDefaultCraftWindow();
    }

    public void SetupCraftList()
    {
        for(int i = 0; i < craftSlotParent.childCount; i++)                   // Kill the old list before you populate a new one
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for(int i = 0; i < craftEquipment.Count; i++)               // For each object in the equipmentlist, instantiate a CraftSlot-prefab. 
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)           // Run SetupCraftList() when you click on the list
    {
        SetupCraftList();
    }

    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
        GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
    }
}
