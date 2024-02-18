using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : UI_Tooltip
{
    // This tooltip object is a child of SkillTree_UI game object. Activated when you hover over a SkillTreeSlot.
    // When you hover over, OnPointerEnter is called which runs UI_SkillTreeSlot.ShowTooltip(). 
    // This function passes the information from that skillslot to this tooltip, and then activates it.
    // Deactivated with HideTooltip() when you hover out. 

    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] private float defaultNameFontSize;

    public void ShowTooltip(string _skillDescription, string _skillName, int _skillCost)
    {
        skillName.text = _skillName;
        skillText.text = _skillDescription;
        skillCost.text = "Cost: " + _skillCost;

        AdjustPosition();
        AdjustFontSize(skillName);

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        skillName.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }
}
