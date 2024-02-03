using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : MonoBehaviour
{
    // This tooltip object is a child of SkillTree_UI game object. Activated when you hover over a SkillTreeSlot.
    // When you hover over, OnPointerEnter is called which runs UI_SkillTreeSlot.ShowTooltip(). 
    // This function passes the information from that skillslot to this tooltip, and then activates it.
    // Deactivated with HideTooltip() when you hover out. 

    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillName;

    public void ShowTooltip(string _skillDescription, string _skillName)
    {
        skillName.text = _skillName;
        skillText.text = _skillDescription;
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
