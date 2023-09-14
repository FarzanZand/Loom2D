using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private float crystalDuration; 
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;


    [Header("Crystal Mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private bool canUseMultiStacks; 
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown; // How long until you can cast multistack crystals again
    [SerializeField] private float useTimeWindow; // How long you can cast crystals before ability goes on cooldown
    [SerializeField] private List<GameObject> crystalsLeft = new List<GameObject>();

    // Potential upgrade: Separate every skill in its own function to make it cleaaaan, cause this code is messy

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal()) 
            return; 

        // Use skill once, crystal spawns on you. Use skill again, you teleport back to crystal and destroy it.Place this in method to clean?
        if(currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)  // You can't teleport to enemy if skill is made to move towards it
                return; 

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if(cloneInsteadOfCrystal) // If you have the Crystal Mirage skill, create clone, don't finish crystal
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
            currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalSkillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform));
    }

    public void CurrentCrystalChooseRandomTarget()
    {
        currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();
    }

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if(crystalsLeft.Count > 0) // You start the scene with crystals loaded
            {
                if (crystalsLeft.Count == amountOfStacks) // Start the casting window when you cast first crystal
                    Invoke("ResetAbility", useTimeWindow);

                cooldown = 0; 
                GameObject crystalToSpawn = crystalsLeft[crystalsLeft.Count - 1]; // Choose last crystal in list
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalsLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<CrystalSkillController>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform));

                if(crystalsLeft.Count <= 0) // If out of crystals, refill, and toggle cooldown
                {
                    cooldown = multiStackCooldown; // change cooldown from parent Skill.cs
                    RefillCrystal(); 

                }
            return true;
            }

        }
        return false;
    }
     

    private void RefillCrystal()
    {
        int amountToAdd = amountOfStacks - crystalsLeft.Count; // Only add the amount we are missing from max stacks, to not go above it

        for(int i = 0; i < amountToAdd; i++) 
        {
            crystalsLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility() // This is created, but never called?
    {
        if (cooldownTimer > 0)
            return; 

        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }
}
