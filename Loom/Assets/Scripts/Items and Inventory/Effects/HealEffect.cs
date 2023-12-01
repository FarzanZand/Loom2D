using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create an item effect, and put this on an equipment. 
// This HealEfect gets a referense to the PlayerStats, gets the healAmount you want to add
// Then runs playerStats.IncreaseHealthBy(healAmount) which heals the player.

[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item effect/Heal effect")]
public class HealEffect : ItemEffect
{

    [Range(0f, 1f)]
    [SerializeField] private float healPercent;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);
        playerStats.IncreaseHealthBy(healAmount);
    }

}
