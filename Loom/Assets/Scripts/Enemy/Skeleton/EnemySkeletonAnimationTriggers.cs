using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{

    private EnemySkeleton enemy => GetComponentInParent<EnemySkeleton>();
    
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
                hit.GetComponent<Player>().Damage();
        }
    }


    // An event that shows an opening for player to counter window. The method in the Enemy.cs 
    // toggles on and off the visual gameobject.
    // Call this event via trigger in animation.
    protected void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    protected void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
