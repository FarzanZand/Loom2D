using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    // 1. When CreateClone() is called via dash, which passes player position, then instantiate´s a clone 
    // 2. The clone is setup via the the method SetupClone() in the CloneSkillController. 
    // 3. Clone can attack if unlocked. See CloneSkillController.cs for more info

    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform));
    }
}
