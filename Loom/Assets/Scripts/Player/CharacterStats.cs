using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    #region Damage Functionality
    // 1. To do damage on player or enemy, in PlayerStats.cs or EnemyStats.cs get access to player.stats
    // 2. in stats, you can DoDamage(), which calculates total damage from character stats. You use that value for TakeDamage()
    // 3. You first check if target evades attack, if not, you take damage and check if crit, if crit, check crit damage, followed by final damage.
    #endregion

    #region Basic Stat Functionality
    // 1. Every stat player or enemy has will be in this class and have its own Stat.cs class, which hold basic stat functionality
    // 2. You can get value of stat from it, and also modify it with for instance, items or buffs. The value returned is after modifiers
    #endregion

    [Header("Major stats")]
    public Stat strength;       // 1 point increase damage vy 1 and crit.power by 1%
    public Stat agility;        // 1 point increase evasion by 1 % and crit chance by 1 %
    public Stat intelligence;   // 1 point increase magic damage by 1 and and magic resistance by 3
    public Stat vitality;       // 1 point increase health by 5 points

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;      // Default value 150 %

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    [SerializeField] int currentHealth;


    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
        critPower.SetDefaultValue(150);
    }

    public virtual void DoDamage(CharacterStats _targetStats) // Do damage by calculating total damage value from stats
    {
        if (TargetCanAvoidAttack(_targetStats)) // Check if damage is evaded. If true, don't take damage
            return;

        int totalDamage = damage.GetValue() + strength.GetValue(); // Get max damage

        if (CanCrit()) // Check if crit and calculate crit damage if so
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage); // Lower damage with armor

        _targetStats.TakeDamage(totalDamage); // Final Damage
        DoMagicalDamage(_targetStats);
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);



    }

    private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked) 
            return;

        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;

    }
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue); // don't go below 0 damage
        return totalDamage;
    }

    private bool  TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true; 
        }

        return false; 
    }

    public virtual void TakeDamage(int _damage) // Takes damage, kills character if < 0. Called via DoDamage(). 
    {
        currentHealth -= _damage;

        Debug.Log(_damage);

        if (currentHealth < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();
        if(Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }
}
