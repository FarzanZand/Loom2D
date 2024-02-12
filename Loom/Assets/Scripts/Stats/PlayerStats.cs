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

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        //  Item effect At X hp
        ItemData_Equipment currentArmor = Inventory.Instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor != null)
        {
            currentArmor.Effect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _attackMultiplier)
    {
        if (TargetCanAvoidAttack(_targetStats)) // Check if damage is evaded. If true, don't take damage
            return;


        int totalDamage = damage.GetValue() + strength.GetValue(); // Get max damage

        if(_attackMultiplier > 0)                           // In clone skill, there is an attack multiplier. 
            totalDamage = Mathf.RoundToInt(totalDamage * _attackMultiplier);

        if (CanCrit()) // Check if crit and calculate crit damage if so
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage); // Lower damage with armor
        _targetStats.TakeDamage(totalDamage); // Final Damage

        // If weapon has elemental damage
        DoMagicalDamage(_targetStats); // Remove if you don't want to apply magic hit on primary attack
    }
}
