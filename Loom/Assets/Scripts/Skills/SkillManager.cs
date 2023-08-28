using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    // 1. Make it a singleton so it is solo, and static so anyplace can access
    // 2. All skills will be placed in this skill manager, so they can be reached by other scripts

    // Skillmanager.cs carries all skill references, making them reachable in game, placed in SkillManager gameobject
    // Skill.cs is the parent class of skill, hold the common functionality of all skills, like, cooldown
    // typeSkill.cs is a child of skill.cs, hold skill-specific information

    public static SkillManager instance;

    public DashSkill dash { get; private set; }
    public CloneSkill clone { get; private set; }
    public  SwordSkill swordThrow { get; private set; }

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
    }
}
