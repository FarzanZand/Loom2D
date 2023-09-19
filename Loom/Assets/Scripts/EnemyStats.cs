using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    
        enemy.DamageEffect();
    }

    protected override void Die() // When current health from CharacterStats.cs is 0, call Die(). 
    {
        base.Die();
        enemy.Die();
    }
}
