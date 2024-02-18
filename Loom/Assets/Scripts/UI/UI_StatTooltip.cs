using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Attach this to the UI.cs script on the canvas gameobject. 
// These functions are called via UI_StatSlot.cs when mouse enters and exits the space. 
// The description is typed in the inspector, on the UI_StatSlot.cs script, once per stat
// Fills textDescription object, which is passed into ShowStatTooltip() as _text. 

public class UI_StatTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI description;

    public void ShowStatTooltip(string _text) 
    {
        description.text = _text;
        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideStatTooltip()
    {
        description.text = "";
        gameObject.SetActive(false);
    }


}
