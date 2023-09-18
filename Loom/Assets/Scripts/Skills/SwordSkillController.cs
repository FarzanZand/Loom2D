using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour

    // The SwordSkillController is the script placed on the instantiated sword
    // Rotates while flying via animation, stops when hitting something
    // from GroundedState.cs, trying the skill again, if no sword held, runs ReturnSword() instead returning sword to player
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private float freezeTimeDuration;
    private float returnSpeed = 20;

    [Header("Pierce info")]
    [SerializeField] private float pierceAmount;

    [Header("Bounce info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount; 
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer; // Timer for how fast damage ticks
    private float hitCooldown;

    private float spinDirection; // Gets direction spinning sword should slowly move towards when spinning

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();  
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    // Generic Setup
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _FreezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _FreezeTimeDuration;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        returnSpeed = _returnSpeed;

        if (pierceAmount <= 0) // Only rotate when not piercing
        anim.SetBool("Rotation", true); // spin-animation while thrown, static when hitting something

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7); // Destroy after 7 seconds
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounces, float _bounceSpeed) 
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounces;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>(); 
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    
    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }
    public void ReturnSword() // Returns sword to player, destroys when near player via update()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // rb.isKinematic = false;
        transform.parent = null;
        isReturning = true; 

    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity; // Puts the point of the sword towards direction / velocity

        if (isReturning) // Return sword to the player and destroy when near
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped) // Make sword stop if it goes far away from player
            {
                StopWhenSpinning(); // Also called when hitting an enemy, sets wasStopped to true, and kickstars spinTimer
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                
                // Makes the sword move slightly forward on hit in he direction of the sword. Remove if you want to
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime; // Setup the damage tick frequency

                if (hitTimer < 0) // Whenever hit-timer runs out, do damage to enemies, than reset hit-timer
                {
                    hitTimer = hitCooldown; // Reset hitTimer

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0) // enemyTarget populated in OnTriggerEnter
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        // Get an enemyTarget-list of all enemies near initial target hit to prepare for bouncing sword. Clean this out perhaps into own function
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    private void StuckInto(Collider2D collision) // When sword hits target and if it should get stuck or not
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning(); // Stop sword movement when hitting an enemy
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return; // Don't end animation while still bouncing

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision) // When sword collides, remove physics and set target as parent 
    {
        if (isReturning)
            return;
        if(collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            SwordSkillDamage(enemy);
        }


        SetupTargetsForBounce(collision);

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.DamageEffect();
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
    }
}
