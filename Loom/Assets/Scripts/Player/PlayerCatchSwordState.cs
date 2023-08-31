using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    // You enter this state after you recall sword and it is near player
    // SwordSkillController.cs when sword is near player, calls CatchTheSword.cs in Player.cs

    private Transform sword;

    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        sword = player.swordForThrowing.transform;

        // Flips the player towards side aimed, if you aim left or right
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (player.transform.position.x > sword.position.x && player.facingDir == 1)
            player.Flip();
        else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
            player.Flip();

        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir, rb.velocity.y); // Knocks back player on catch
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f); // Keep the player from moving wierdly when catching, so player stands still first
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
