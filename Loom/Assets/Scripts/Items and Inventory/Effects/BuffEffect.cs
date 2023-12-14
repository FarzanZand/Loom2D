using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Buff effect buffs adds a modifier to any of the characterstats. 
// When effect is executed, it gets a reference to the player, and runs IncreaseStatBy()
// This function Adds a modifier for X amount of time for chosen stat, and then removes that modifier after it.
// Good for flask, or on hit. 

[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
public class BuffEffect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private int floatDuration;  

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount, floatDuration, stats.GetStat(buffType));
    }
}
