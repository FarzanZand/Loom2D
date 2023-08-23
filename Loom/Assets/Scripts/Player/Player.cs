using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    [Header("Attack Details")]
    public Vector2[] attackMovement;

    public bool isBusy {  get; private set; }
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown; // Deleting this and usage timer later
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration; 
    public float dashDir { get; private set; }







    #region states
    // Every state created for the player needs to be declared here
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState  moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }  
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    #endregion 

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump"); // Use jump animation
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger(); // Calls Animation Finish Trigger in the current state

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return; // Don't dash when in wall.

        dashUsageTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown; // Resets timer which counts down after key is pressed. Can only be pressed when > 0. 
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }
}
