using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// This script is attached to the Canvas object. 
// The buttons in the UI that swap between views are connected to the SwitchTo() function here
// SwitchTo has the four core buttons on top "Character, skill tree, craft, options.
// When triggered, it deactivates everything except the button that you clicked on to active it
// The Canvas objects (i.e. character_UI, or craft_UI) all have a bar on them with all other UIs
// These are buttons. When they are clicked, they pass a reference of what was clicked to the SwitchTo()

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
    [SerializeField] private GameObject inGameUI;

    public UI_SkillTooltip skillTooltip;
    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statTooltip;
    public UI_CraftWindow craftWindow;

    private void Awake()
    {
        SwitchTo(skillTreeUI);          // We need this to assign events on skill tree slots before we assign events on skill scripts.  
    }

    void Start()
    {
        SwitchTo(inGameUI);   

         
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
        if (_menu != null && _menu.activeSelf)                  // If menu is already active, close it. Otherwise, run SwitchTo(_menu) and open it, closing all else. 
        {
            _menu.SetActive(false);
            CheckForInGameUI();                                 // Activate InGameUI if everything is off
            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()                             // If no UI-menu is active, activate inGameUI. InGameUI is deactivated in SwitchTo() when menus open.
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return;
        }

        SwitchTo(inGameUI);
    }
}
