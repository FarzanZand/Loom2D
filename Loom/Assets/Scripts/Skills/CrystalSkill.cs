using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private float crystalDuration; 
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal; 

    public override void UseSkill()
    {
        base.UseSkill();

        // Use skill once, crystal spawns on you. Use skill again, you transform back to crystal and destroy it.
        if(currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            CrystalSkillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();

            currentCrystalScript.SetupCrystal(crystalDuration); 
        }
        else
        {
            player.transform.position = currentCrystal.transform.position;
            Destroy(currentCrystal);
        }
    }
}
