using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// This is attached to the Canvas Object Character - Stats - Major Stats - Stat. Basically, the object in the UI showing the stat value and name.
// These statObjects are placed in a GridLayoutGroup for easy organizing
// They get their name from the StatType enum in CharacterStats
// At start, you run UpdateStatValueUI(), which gets a reference to the PlayerStat function GetStat(), which returns the value of the statType. UpdateStatValueUI() also runs everytime you change equipment.
// Some stats have modifiers, like crit chance is based on base crit and agility. They are calculated before text is changed to show this true number
// Same logic is used for UI_StatTooltip for its tooltip, but it is instead attached to the stat object in the interface, and not the itembox. Check UI_ItemTooltip.cs for details

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;         // Dragged in to the inspector
    [SerializeField] private TextMeshProUGUI statNameText;          // Dragged in to the inspector

    [TextArea]
    [SerializeField] private string statDescription;                // Type in the inspector the stat description


    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;
        if (statNameText != null)
            statNameText.text = statName;
    }

    private void Start()
    {
        UpdateStatValueUI();
        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();

            if(statType == StatType.health)
                statValueText.text = playerStats.GetMaxHealthValue().ToString();

            if(statType == StatType.damage)
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();

            if (statType == StatType.critPower)
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();

            if (statType == StatType.critChance)
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();

            if (statType == StatType.evasion)
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();

            if (statType == StatType.magicRes)
                statValueText.text = (playerStats.magicResistance.GetValue() + playerStats.intelligence.GetValue() * 3).ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTooltip.ShowStatTooltip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltip.HideStatTooltip();
    }
}
