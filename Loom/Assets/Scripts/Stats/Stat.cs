using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Will now be visible in inspector. Stat-object created will show all values.For instance, Stat damage or maxHealth;
public class Stat
{

    [SerializeField] private int baseValue; 

    public List<int> modifiers;

    public int GetValue()
    { 
        int finalValue = baseValue;

        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }
        return finalValue; 
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }
    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifiers.RemoveAt(_modifier);
    }
}
