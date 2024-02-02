using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

// Every equipment has an equipment type, and when you equip an item from inventory, it is removed from the inventory
// and placed in the equipment slot, while removing the old equipment and placing that in inventory. Check inventory.cs for more info.
// Equipment has stats. in Inventory.cs, AddModifiers() is called when you equip, RemoveModifiers() is called when you unequip.

// We reach stats via PlayerStats playerStats, which is a child of the parent CharacterStats, holding all different stat-objects. 
// Every stat its own object and reached via playerStats.statName. Defined in CharacterStats.cs, which created object per stat based from the Stat.cs class. 
// When you add a modifier of a stat with AddModifiers(), you populate the List<int> modifiers in stat.cs of that stat. 
// The final value is calculated using GetValue() in Stat.cs. It takes base value, and adds all modifiers to it. 
// RemoveModifier() does the reverse, just removes it from the list. 

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet, 
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique effect")]
    public float itemCooldown;
    public ItemEffect[] itemEffects;
    [TextArea]
    public string itemEffectDescription;

    [Header("Major stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offensive stats")]
    public int damage;
    public int critChance;
    public int critPower;

    [Header("Defensive stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;

    [Header("Craft Requirements")]
    public List<InventoryItem> craftingMaterials; // Populated in the inspector

    private int descriptionLength;

    public void Effect(Transform _enemyPosition)
    {
        foreach(var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }

    public override string GetDescription() // Gets the description for the text in the tooltip. Go through every stat. Every non-zero stat gets added to tooltip and adds description length for line/size of window.
    {
        stringBuilder.Length = 0;
        descriptionLength = 0; 

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit.Chance");
        AddItemDescription(critPower, "Crit.Power");

        AddItemDescription(health, "Health");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(armor, "Armor");
        AddItemDescription(magicResistance, "Magic Resist.");

        AddItemDescription(fireDamage, "Fire damage");
        AddItemDescription(iceDamage, "Ice damage");
        AddItemDescription(lightningDamage, "Lightning dmg. ");

        if (descriptionLength < 5)          // Add a minimum length to the tooltip window, so it isn't too small if there is little info. 
        {
            for (int i = 0; i < 5 - descriptionLength; i++) // Add lines so you reach 5 lines total. 
            {
                stringBuilder.AppendLine();
                stringBuilder.Append("");
            }
        }

        if (itemEffectDescription.Length > 0)
        {
            stringBuilder.AppendLine();
            stringBuilder.Append(itemEffectDescription);
        }

        return stringBuilder.ToString();
    }

    private void AddItemDescription(int _value, string _name)   
    {
        if (_value != 0)
        {
            if (stringBuilder.Length > 0)              // value is 0 on the item stat, then do not show it in the tooltip. 
                stringBuilder.AppendLine();

            if(_value > 0)
                stringBuilder.Append("+ " + _value + " " + _name);

            descriptionLength++;            // Everytime we add a line (every iteration of value more than 0), we add a value. If we have added 5 or less lines, we have a at least 5 lines.
        }
    }
}

