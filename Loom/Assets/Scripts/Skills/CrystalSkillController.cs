using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{

    // 1. Spawns a crystal on player that can be morped to various skills via talent tree
    // 2. Press F, coded in Player.cs to use skill.crystal.CanUseSkill();
    // 3. CrystalSkill.cs instantiates crystal and sets up CrystalController
    // 4. If you already have a crystal in game, player teleports to crystal instead with F
    // 5. Crystal has a timer and does a FinishCrystal-event after timer depending on talent chosen, ends with SelfDestroy()

    // 6. If canExplode, finishCrystal runs explode anim with event for AnimationExplodeEvent(), which grows and explodes crystal
    // 7. If canMove, use FindClosestEnemy() from skill.cs, and MoveTowards() that target. FinishCrystal() when near it
    // 8. if canUseMultiStacks, return UseSkill early and run CanUseMultiCrystal() which has different function for crystal
    // 9. You start the scene with crystals loaded, CanUseMultiCrystals() instantiates crystal on action. You can cast several that explode
    // 10. When you cast a crystal, a cooldown window starts, giving you time to cast remaining crystals. When 0 left, refill crystal. 
    

    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D circleCollider => GetComponent<CircleCollider2D>();

    private float crystalExistTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;

    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestTarget)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if(colliders.Length > 0)
        {
        closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }
    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0) // end crystal if timer runs out
        {
            FinishCrystal();
        }

        if (canMove) // Move towards target, then finish crystal when near, go boom
        {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                FinishCrystal();
                canMove = false;
            }
        }


        if (canGrow) // Grow the explosion
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().DamageEffect();
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    } 

}
