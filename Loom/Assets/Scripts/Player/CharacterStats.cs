using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    #region Damage Functionality
    // 1. To do damage on player or enemy, in PlayerStats.cs or EnemyStats.cs get access to player.stats
    // 2. in stats, you can DoDamage(), which calculates total damage from character stats. You use that value for TakeDamage()
    #endregion

    #region Basic Stat Functionality
    // 1. Every stat player or enemy has will be in this class and have its own Stat.cs class, which hold basic stat functionality
    // 2. You can get value of stat from it, and also modify it with for instance, items or buffs. The value returned is after modifiers
    #endregion

    public Stat strength;
    public Stat damage;
    public Stat maxHealth; 

    [SerializeField] int currentHealth;


    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamage(CharacterStats _targetStats) // Do damage by calculating total damage value from stats
    {

        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStats.TakeDamage(totalDamage);
    }
    public virtual void TakeDamage(int _damage) // Takes damage, kills character if < 0. Called via DoDamage(). 
    {
        currentHealth -= _damage;

        Debug.Log(_damage);

        if (currentHealth < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        //throw new NotImplementedException();
    }
}
