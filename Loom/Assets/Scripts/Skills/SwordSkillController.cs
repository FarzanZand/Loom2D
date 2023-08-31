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

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();  
        cd = GetComponent<CircleCollider2D>();


    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        anim.SetBool("Rotation", true); // spin-animation while thrown, static when hitting something
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
        if(canRotate)
        transform.right = rb.velocity; // Puts the point of the sword towards direction / velocity

        if (isReturning) // Return sword to the player and destroy when near
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }


    }

    private void OnTriggerEnter2D(Collider2D collision) // When sword collides, remove physics and set target as parent 
    {
        if (isReturning)
            return;

        anim.SetBool("Rotation", false);
        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
    }
}
