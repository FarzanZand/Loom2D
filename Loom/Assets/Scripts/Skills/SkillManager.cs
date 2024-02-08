using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    // 1. Make it a singleton so it is solo, and static so anyplace can access
    // 2. All skills will be placed in this skill manager, so they can be reached by other scripts

    // Skillmanager.cs carries all skill references, making them reachable in game, placed in SkillManager gameobject
    // Skill.cs is the parent class of skill, hold the common functionality of all skills, like, cooldown
    // typeSkill.cs is a child of skill.cs and is attached to SkillManager, what you need to setup the skill is here
    // When it is in the SkillManager, you can call the skill from anywhere with PlayerManager.Instance.skillName
    // typeSkillController.cs is the logic for the skill itself

    // Showing cassie object oriented programming

    public static SkillManager instance;

    public DashSkill dash { get; private set; }
    public CloneSkill clone { get; private set; }
    public  SwordSkill swordThrow { get; private set; }
    public BlackholeSkill blackhole { get; private set; }
    public CrystalSkill crystal { get; private set; }
    public ParrySkill parry { get; private set; }
    public DodgeSkill dodge { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        swordThrow = GetComponent<SwordSkill>();
        blackhole = GetComponent<BlackholeSkill>();
        crystal = GetComponent<CrystalSkill>();
        parry = GetComponent<ParrySkill>();
        dodge = GetComponent<DodgeSkill>();
    }
}
