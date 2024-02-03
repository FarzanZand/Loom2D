using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    // 1. When CreateClone() is called via dash, which passes player position, then instantiate´s a clone 
    // 2. The clone is setup via the the method SetupClone() in the CloneSkillController. 
    // 3. Clone can attack if unlocked. See CloneSkillController.cs for more info

    // 4. If createCloneOnDashStart is true, in PlayerDashState.cs enter method, run CreateCloneOnDashStart()
    // 5. Same logic as above for createCloneOnDashOver;
    // 6. For createCloneWithCounter (from PlayerCounterAttackState.cs,if true, CreateCloneOnCounterAttack() runs to completion. 
    // 7. canDuplicateClone, if true, on hit, do random range and if success, CreateClone()
    // 8. crystalInsteadOfClone, when CreateClone(), spawn crystal at start and return before clone is instantiated

    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration; 
    [Space]
    [SerializeField] private bool canAttack;

    [SerializeField] private bool canCreateCloneOnCounterAttack;
    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {

        // Create a crystal instead of a clone
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return; 
        }


        // Create a clone
            GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate, player);
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform) // Called in PlayerCounterAttackState.cs
    {
        if (canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
            CreateClone(_transform, _offset);
    }
}
