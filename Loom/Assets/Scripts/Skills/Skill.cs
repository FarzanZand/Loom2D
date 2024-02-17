using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    // TODO: In the future, swap NameSkill with NameSkillController, reverse it. Makes more sense the other way around
    // Skills have cooldown that ticks down with deltaTime, and as long as cooldown is up CanUseSkill() does not trigger UseSkill(). 

    public float cooldown;
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if(cooldownTimer < 0) // cooldownTimer ticks down from skill-cooldown via deltaTime. If < cooldown, do skill.
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        Debug.Log("Skill is on Cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        // Do some skill specific things, defined more in the child UseSkill
    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform) // Finds the closest enemy to _checkTransform
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null) // Sort enemies, get the closest one
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }

}
