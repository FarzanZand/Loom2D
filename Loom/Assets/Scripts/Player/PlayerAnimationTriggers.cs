using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerAnimationTriggers : MonoBehaviour
{

    // Trigger logic for misc events DEMO COMMENT
    // 1. Set the animation clip to call AnimationTrigger() as an event
    // 2. This will be passed to the player, which will call that function in current playerState 
    // 3. That function will in turn call AnimationFinishTrigger(), which setting triggerCalled to true.
    // 4. When triggerCalled is true, the update() can check for it and do something with it
    // 5. triggerCalled is reset to false in enter-method when you enter a state. 

    // This will for instance in playerAttack1 return the player to idle when triggerCalled is true. A check done in update.

    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
    
    // AttackLogic: 
    // 1. Entity has AttackCheck game object placed at location of attack hit. 
    // 2. Animation has an event trigger which calls this method AttackTrigger()
    // 3. AttackTrigger() does an aoe check for all colliders within it
    // 4. For all colliders with the component Enemy, call the function Damage() from enemy parent class Entity
    // 5. Logic when damaged is applied in that function
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats target = hit.GetComponent<EnemyStats>();

                player.stats.DoDamage(target);



                ItemData_Equipment weaponData = Inventory.Instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null)
                    weaponData.Effect(target.transform);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.swordThrow.CreateSword();
    }
}
