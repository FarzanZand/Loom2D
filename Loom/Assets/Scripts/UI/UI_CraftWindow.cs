using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;

    [SerializeField] private Image[] materialImage;

    public void SetupCraftWindow(ItemData_Equipment _data)              // Setup the materials in the list
    {
        for (int i = 0; i < materialImage.Length; i++)                  // each of the 1-4 materialslots, dragged in from inspector. 
        {
            materialImage[i].color = Color.clear;                       // Clear the window, making all four windows and text transparent/hidden for setup.
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < _data.craftingMaterials.Count; i++)         // Get the amount of different types of craftingMaterials needed. 1-4
        {
            if (_data.craftingMaterials.Count > materialImage.Length)   // If more than 4, error message
                Debug.LogWarning("You have more material amount than you have material slots in craft window. 4 max.");

            // Per iteration, fill icon data
            materialImage[i].sprite = _data.craftingMaterials[i].data.itemIcon; // Add image icon
            materialImage[i].color = Color.white;                               // Make it visible

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.text = _data.craftingMaterials[i].stackSize.ToString();    // Get the stack size needed to craft
            materialSlotText.color = Color.white;                                       // Make it visible
        }
    }
}
