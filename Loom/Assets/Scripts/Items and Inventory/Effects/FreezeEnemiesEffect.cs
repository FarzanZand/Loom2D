using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Freezes enemies within radius of 2. If the armor is on cooldown, effect will not happen. 
// If HP is above 20 %, effect will not happen as defined in ExecuteEffect()
// Currently set in PlayerStats where the effect is triggered in DecreaseHealthBy() function

[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item effect/Freeze enemies")]
public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float duration;
    [SerializeField] private float freezeWhenBelowThisHealthPercentage; 

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * freezeWhenBelowThisHealthPercentage)
            return; // Only trigger if you have less than this Hp


        if (!Inventory.Instance.CanUseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration); // The ? Makes sure the component exists
        }
    }
}
