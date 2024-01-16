using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;             // The CraftSlotsParent gameobject in the canvas craft viewport. Drag in inspector
    [SerializeField] private GameObject craftSlotPrefab;            // CraftSlotPrefab created, drag in from assets

    [SerializeField] private List<ItemData_Equipment> craftEquipment;
    [SerializeField] private List<UI_CraftSlot> craftSlots; 
    void Start()
    {
        AssignCraftSlots();
    }

    private void AssignCraftSlots()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)        // At start, get the current list of all craftSlots and add it to the list. Will be killed on refresh.
        {
            craftSlots.Add(craftSlotParent.GetChild(i).GetComponent<UI_CraftSlot>());
        }
    }

    public void SetupCraftList()
    {
        for(int i = 0; i < craftSlots.Count; i++)                   // Kill the old list before you populate a new one
        {
            Destroy(craftSlots[i].gameObject);
        }

        craftSlots = new List<UI_CraftSlot>();

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
}
