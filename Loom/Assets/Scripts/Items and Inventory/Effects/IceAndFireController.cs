using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFireController : MonoBehaviour
{

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();

            playerStats.DoMagicalDamage(enemyTarget);
            // Use enemyTarget.TakeDamage(value); if you want to do clean damage without modifiers
        }
    }

}



/*
 * public class IceAndFireController : ThunderStrikeController 
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

}

He has ThunderStrikeController.cs as parent. At this stage, feels redundant.
*/