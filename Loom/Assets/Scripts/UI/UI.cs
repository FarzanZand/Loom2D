using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// This script is attached to the Canvas object. 
// The buttons in the UI that swap between views are connected to the SwitchTo() function here
// SwitchTo has the four core buttons on top "Character, skill tree, craft, options.
// When triggered, it deactivates everything except the button that you clicked on to active it

// For switching on and off the UI with hotkeys, each UI-gameobject is dragged into these variables in the inspector
// When you click the hotkey, it passes a reference to the UI gameobject in the canvas (i.e. characterUI) to SwitchWithKeyTo()
// This closes the passed _menu if already active. 
// If it is closed, it calls SwitchTo() which closes everything except your _menu.

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;

    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statTooltip;

    void Start()
    {
        SwitchTo(null);   
        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(craftUI);
        
        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(optionsUI);
    }
    public void SwitchTo(GameObject _menu)                      // Called via button press in canvas
    {
        for (int i = 0; i < transform.childCount; i++)          // Deactivate everything that is not the trigger (_menu) 
        {
           transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);                              // Activate the thing that triggered this function (_menu)
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)               // Close menu if already active, or call SwitchTo to open it. 
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            return;
        }

        SwitchTo(_menu);
    }
}
