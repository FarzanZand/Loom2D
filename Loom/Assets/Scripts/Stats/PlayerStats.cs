using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;


    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die() // called in CharacterStats.cs when hp is 0. player.Die() starts playerDeadState.cs
    {
        base.Die();
        player.Die();

        GetComponent <PlayerItemDrop>()?.GenerateDrop(); // If it is not null, GenerateDrop(); Drop items by chance from player death TODO delete later no fun
    }
}
