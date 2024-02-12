using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private float attackMultiplier; 
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]

    [Header("Clone attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier; 
    [SerializeField] private bool canAttack;

    [Header("Aggressive clone")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggressiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }

    [Header("Multiple Clones")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    [Header("Crystal instead of clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }

    #region Unlock Region
    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockAggressiveClone() // rename me? Yes
    {
        if(aggressiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneAttackMultiplier;
        }
    }

    private void UnlockMultiClone()
    {
        if (multipleUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }

    private void UnlockCrystalInstead()
    {
        if(crystalInsteadUnlockButton.unlocked)
        {
            crystalInsteadOfClone = true;
        }
    }

    #endregion 

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
        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate, player, attackMultiplier);
    }

    public void CreateCloneWithDelay(Transform _enemyTransform) // Called in ParrySkill.cs if talent is unlocked. 
    {
            StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CloneDelayCoroutine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
            CreateClone(_transform, _offset);
    }
}
