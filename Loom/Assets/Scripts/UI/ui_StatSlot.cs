using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// This is attached to the Canvas Object Character - Stats - Major Stats - Stat. Basically, the object in the UI showing the stat value and name.
// These statObjects are placed in a GridLayoutGroup for easy organizing
// They get their name from the StatType enum in CharacterStats
// At start, you run UpdateStatValueUI(), which gets a reference to the PlayerStat function GetStat(), which returns the value of the statType. UpdateStatValueUI() also runs everytime you change equipment.

public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private string statName;

    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;         // Dragged in to the inspector
    [SerializeField] private TextMeshProUGUI statNameText;          // Dragged in to the inspector

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;
        if (statNameText != null)
            statNameText.text = statName;
    }

    private void Start()
    {
        UpdateStatValueUI();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();
        }
    }
}
