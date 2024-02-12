using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    // 1. This is added to the Clone prefab, and controls it while it exists
    // 2. Lives as long as cloneDuration which is passed to it's SetupClone() via CloneSkill.cs
    // 3. SetupClone sets up the attack which should run, which via AttackTrigger() checks collider
    // 4. Faces closest target that is within the list of colliders
    // 5. After cloneTimer runs out and color.a is 0, destroy gameObject.

    private Player player;
    [SerializeField] private float colorLoosingSpeed;
    private SpriteRenderer sr;
    private Animator anim;


    private float cloneTimer;
    private float attackMultiplier; 
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8f;
    private Transform closestEnemy;
    private int facingDir = 1; 

    private bool canDuplicateClone;
    private float chanceToDuplicate;
    private void Awake()
    {
     sr = GetComponent<SpriteRenderer>();   
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
            if (sr.color.a <= 0)
                Destroy(gameObject);
        }
    }
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicateClone, float _chanceToDuplicate, Player _player, float _attackMultiplier)
    { 
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));

        attackMultiplier = _attackMultiplier;

        player = _player;

        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;
        FaceClosestTarget();
    }
    private void AnimationTrigger() // cloneTimer when < 0, Destroy object. This method called via anim event
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger() // Attacks enemys within collider tagged Enemy. Called via event in anim clip
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attackMultiplier);
                
                if (player.skill.clone.canApplyOnHitEffect)                 // Gets the equipment hit effect from the weapon
                {
                    ItemData_Equipment weaponData = Inventory.Instance.GetEquipment(EquipmentType.Weapon);

                    if (weaponData != null)
                        weaponData.Effect(hit.transform);
                }

                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate) // Percent-chance to duplicate
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(0.5f * facingDir, 0));
                    }
                }

            }
        }
    }

    private void FaceClosestTarget() // Get all enemies in collider, with foreach, get the closest one, rotate if needed
    {
        if(closestEnemy != null) // closestEnemy found in setup function
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
