using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    #region Basic Stat Functionality
    // 1. Every stat player or enemy has will be in this class and have its own Stat.cs class, which hold basic stat functionality
    // 2. You can get value of stat from it, and also modify it with for instance, items or buffs. The value returned is after modifiers
    #endregion

    public Stat damage;
    public Stat maxHealth; 

    [SerializeField] int currentHealth;


    void Start()
    {
        currentHealth = maxHealth.GetValue();


        
        damage.AddModifier(4); // Example, adds four to damage
    }

    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}
