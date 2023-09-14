using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkill : Skill
{
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    BlackholeSkillController currentBlackhole;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        
        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        
        currentBlackhole = newBlackHole.GetComponent<BlackholeSkillController>();

        currentBlackhole.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown, blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if(!currentBlackhole)
            return false;

        if(currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true; 
        }

        return false;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2; // This will make the radius return be equal to the black hole since collider radius is 0.5
    }
}
