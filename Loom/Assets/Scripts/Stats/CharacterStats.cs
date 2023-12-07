using System.Collections;
using Unity.Hierarchy;
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

    #region Ailments
    // 1. If you apply elemental damage, depending on skill or gear, the magic begins in DoMagicalDamage()
    // 2. The elemental damage with highest damage gets the effect, random if all are equal. If effect happens, it is applied in ApplyAilments()
    // 3. ApplyAilments() sets the ailment as active and also resets the ailmentTimer.
    // 4. ignite works in Update(), chilled works in CheckTargetArmor(), shock works in TargetCanAvoidAttack(). 
    // 5. Chill slows through overriden SlowEntityBy() in Entity.cs child class enemy or player.
    // 6. Shock lowers dodge, and if you hit an already shocked enemy you send a shockstrike nearest enemy from target with HitNearestTargetWithShockStrike().
    // 7. If nearest enemy != null, instantiate a shockstrike-prefab with ShockStrikeController.cs. ShockStrike moves towards target, and when hit, does damage + anim. 
    #endregion

    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength;                            // 1 point increase damage vy 1 and crit.power by 1%
    public Stat agility;                             // 1 point increase evasion by 1 % and crit chance by 1 %
    public Stat intelligence;                        // 1 point increase magic damage by 1 and and magic resistance by 3
    public Stat vitality;                            // 1 point increase health by 5 points

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;                           // Default value 150 %

    [Header("Defensive stats")]
    public Stat maxHealth;                           // Should rename to baseHealth, and then create a maxHealth that adds base + vitality. Also update GetMaxHealthValue(). 
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    [SerializeField] private float ailmentsDuration = 4;
    public bool isIgnited;                           // Damage over time through Update()
    public bool isChilled;                           // Decreases armor by 20 % through CheckTargetArmor() and slows by 20 %.
    public bool isShocked;                           // Reduce accuracy by 20 % through targetCanAvoidAttack()

    private float ignitedTimer;                      // How long effect will last
    private float chilledTimer;                      // Perhaps combine as ailmentTimer and make it a public variable? 
    private float shockedTimer;

    private float igniteDamageCooldown = 0.39f;      // timer tick between each burn
    private float igniteDamageTimer;                 // The timer that counts down and gets reset, speed of the dot
    private int   igniteDamage;                      // the damage each ignite-tick will do
    private int shockDamage;
    [SerializeField] private GameObject shockStrikePrefab;

    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead { get; private set; } 


    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }


    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;


        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if(isIgnited)
        ApplyIgniteDamage();
    }

    
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }       // For item effects such as buffs that temp increase stats

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration); 
        _statToModify.RemoveModifier(_modifier);
    }
    
    // DAMAGE CALCULATIONS
    #region Damage calculations
    public virtual void DoDamage(CharacterStats _targetStats) // Do damage by calculating total damage value from stats. Target aquired from AnimationTrigger() damage, which checks for targets and passes it here
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
        
        // If weapon has elemental damage
        DoMagicalDamage(_targetStats); // Remove if you don't want to apply magic hit on primary attack
    }

    // If I want to do specific damage regardless of playerstats or modifiers
    public virtual void DoSpecificPhysicalDamage(CharacterStats _targetStats, int _damageValue)
    {
        int totalDamage = CheckTargetArmor(_targetStats, _damageValue); // Lower damage with armor
        _targetStats.TakeDamage(totalDamage); // Final Damage
    }

    // If I want to do specific magical damage regardless of playerstats or modifiers
    public virtual void DoSpecificMagicalDamage(CharacterStats _targetStats, int _damageValue, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage;

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);
        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }
    #endregion

    // MAGICAL DAMAGE AND AILMENTS
    #region Magical Damage and Ailments
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);

        // The elemental with highest damage deals its effect
        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0) // Return if all elements do 0 damage
            return;

        // Element with highest damage gets the effect through
        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    } // Do magical damage. Below we do ailments

    private void AttemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;           // Activated in Update()
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;             // Activated in CheckTargetArmor()
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;       // Activated in TargetCanAvoidAttack()

        // if all damages are equal, decides which to apply
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.33f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.3f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.3f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite) 
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));                   // ignite does 20 % of fire damage per tick

        if (canApplyShock) 
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * 0.1f));         // Does lightning damage effect

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;                               // Don't ignite someone already ignited or affected by something else
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFxFor(ailmentsDuration);
        }
        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked) // Shock if not shocked, lowering dodge
            {
                ApplyShock(_shock);
            }

            else // if already shocked, send a shockStrike to someone else. 
            {
                if (GetComponent<Player>() != null)
                    return;

                HitNearestTargetWithShockStrike();
            }
        }
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = ailmentsDuration;

        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1) // Sort enemies, get the closest one
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
            {
                closestEnemy = transform;
            }
        }
        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrikeController>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);
            if (currentHealth < 0 && !isDead)
                Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
    }
    public void SetupIgniteDamage(int _damage)
    {
        igniteDamage = _damage; 
    }
    public void SetupShockStrikeDamage(int _damage)
    {
        shockDamage = _damage;
    }

    #endregion

    // STAT CALCULATIONS
    #region Stat calculations, crit, armor, dodges and resistances etc
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
        totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue); // don't go below 0 damage
        return totalDamage;
    }
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }
    private bool  TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;


        if (Random.Range(0, 100) < totalEvasion)
        {
            return true; 
        }

        return false; 
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

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5; 
    }
    #endregion

    // INCREASE OR DECREASE HEALTH
    #region Increase or decrease health
    public virtual void TakeDamage(int _damage) // Takes damage, kills character if < 0. Called via DoDamage(). 
    {
        DecreaseHealthBy(_damage);
        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth < 0 && !isDead)
        {
            Die();
        }


    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;
        if(currentHealth > GetMaxHealthValue())
        {
            currentHealth = GetMaxHealthValue();
        }

        if (onHealthChanged != null)
            onHealthChanged();
    }
    protected virtual void DecreaseHealthBy(int _damage) 
    {
        // DecreaseHealthBy(), If you want to decrease health without doing anything else. 
        // TakeDamage() kills at 0 and does FX, good for being hit. This is for instance for items decrasing health or burn damage. 

        currentHealth -= _damage;
        if (onHealthChanged != null)
            onHealthChanged();
    }
    #endregion
    protected virtual void Die()
    {
        isDead = true; 
    }
}
