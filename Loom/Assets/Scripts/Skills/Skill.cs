using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{

    [SerializeField] protected float cooldown;
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
        // Do some skill specific things
    }

}