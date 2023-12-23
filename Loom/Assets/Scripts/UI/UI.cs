using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// This script is attached to the Canvas object. 
// The buttons in the UI that swap between views are connected to the SwitchTo() function here
// SwitchTo has the four core buttons on top "Character, skill tree, craft, options.
// When triggered, it deactivates everything except the button that you clicked on to active it

public class UI : MonoBehaviour
{
    public UI_ItemTooltip itemTooltip;

    public void SwitchTo(GameObject _menu)
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
}
