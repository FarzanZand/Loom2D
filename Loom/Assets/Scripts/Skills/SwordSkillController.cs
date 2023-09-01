using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour

    // The SwordSkillController is the script placed on the instantiated sword
    // Rotates while flying via animation, stops when hitting something
    // from GroundedState.cs, trying the skill again, if no sword held, runs ReturnSword() instead returning sword to player
{
    [SerializeField] private float returnSpeed = 20;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    [Header("Pierce info")]
    [SerializeField] private float pierceAmount;

    [Header("Bounce info")]
    [SerializeField] private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount; 
    private List<Transform> enemyTarget;
    private int targetIndex; 

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();  
        cd = GetComponent<CircleCollider2D>();


    }

    // Generic Setup
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if(pierceAmount <= 0) // Only rotate when not piercing
        anim.SetBool("Rotation", true); // spin-animation while thrown, static when hitting something
    }

    // Bounce
    public void SetupBounce(bool _isBouncing, int _amountOfBounces) 
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounces;

        enemyTarget = new List<Transform>(); 
    }

    // Pierce
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    public void ReturnSword() // Returns sword to player, destroys when near all via update
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
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
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

    private void OnTriggerEnter2D(Collider2D collision) // When sword collides, remove physics and set target as parent 
    {
        if (isReturning)
            return;

        collision.GetComponent<Enemy>()?.Damage(); // ? is like an if-statement, but shorter.

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

        StuckInto(collision);
    }

    private void StuckInto(Collider2D collision)
    {
        if(pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
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
}
