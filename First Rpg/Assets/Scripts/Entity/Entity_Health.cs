using System;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour,IDamagable
{
    public event Action OnTackingDamage;
    public event Action OnHealthUpdate;

    private Slider healthBar;
    private Entity entity;
    private Entity_VFX entityVfx;
    private Entity_Stats entityStats;
    private Entity_DropManager dropManager;

    private bool miniHealthBarActive;
    [SerializeField] protected float currentHealth;
    [Header("Health regen")]
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private bool canRegenerateHealth = true;
    public float lastDamageTaken { get; private set; }
    public bool isDead { get; private set; }
    protected bool canTakeDamage = true;

    [Header("On Damage Knockback")]
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private Vector2 onDamageKnockback = new Vector2(1.5f, 2.5f);
    [Header("On Heavy Damage Knockback")]
    [Range(0, 1)]
    [SerializeField] private float heavyDamageThreshold=0.3f;
    [SerializeField] private float heacyKnockbackDuration = 0.5f;
    [SerializeField] private Vector2 onHeavyDamageKnockback = new Vector2(7, 7);


    protected virtual void Awake()
    {
        entity=GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
        healthBar = GetComponentInChildren<Slider>();
        entityStats=GetComponent<Entity_Stats>();
        dropManager = GetComponent<Entity_DropManager>();

    }
    protected virtual void Start()
    {
        SetupHealth();

    }


    private void SetupHealth()
    {
        if(entityStats==null)
            return;

        currentHealth = entityStats.GetMaxHealth();
        OnHealthUpdate += UpdateHealthBar;

        UpdateHealthBar();
        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }

    public virtual bool TakeDamage(float damage,float elementalDamage,ElementType element,  Transform damageDealer)
    {
        if (isDead||canTakeDamage==false)
            return false;

        if (AttackEvaded())
        {
            Debug.Log("miss");
            return false;
        }

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;
        float mitigation = entityStats!=null? entityStats.GetArmorMitigation(armorReduction):0;
        float resistance = entityStats != null ? entityStats.GetElementalResistance(element) : 0;

        float physicalDamageTaken = damage * (1 - mitigation);
        float elementalDamageTaken = elementalDamage * (1 - resistance);

        TakeKnockback(damageDealer, physicalDamageTaken);
        ReduceHealth(physicalDamageTaken + elementalDamageTaken);

        lastDamageTaken = physicalDamageTaken+ elementalDamageTaken;

        OnTackingDamage?.Invoke();
        return true;
    }
    public void SetCanTakeDamage(bool canTakeDamage)=> this.canTakeDamage = canTakeDamage;
    private bool AttackEvaded()
    {
        if(entityStats==null)
            return false;
        else
            return UnityEngine.Random.Range(0, 100) < entityStats.GetEvasion();  
    }

    private void RegenerateHealth()
    {
        if(canRegenerateHealth==false)
            return;

        float regenAmount = entityStats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }

    public void IncreaseHealth(float healAmount)
    {
        if(isDead)
            return;

        float newHealth = currentHealth + healAmount;
        float maxHealth = entityStats.GetMaxHealth();

        currentHealth = Mathf.Min(newHealth,maxHealth);
        OnHealthUpdate?.Invoke();
        
    }

    public  void ReduceHealth(float damage)
    {
        currentHealth -=damage;

        entityVfx?.PlayOnDamageVfx();
        OnHealthUpdate?.Invoke();

        if (currentHealth < 0)
            Die();
            
    }
    protected virtual void Die()
    {
        isDead = true;
        entity.EntityDeath();
        dropManager?.DropItems();
    }

    public float GetHeakthPercent() => currentHealth / entityStats.GetMaxHealth();
    public void SetHealthToPercent(float percent)
    {
        currentHealth = entityStats.GetMaxHealth()* Mathf.Clamp01(percent);
        OnHealthUpdate?.Invoke();
    }
    public float GetCurrentHealth()=> currentHealth;

    private void UpdateHealthBar()
    {
        if (healthBar == null && healthBar.transform.parent.gameObject.activeSelf==false)
            return;

        healthBar.value = currentHealth / entityStats.GetMaxHealth(); 
    }
    public void EnabelHealthBar(bool enable)=>healthBar?.transform.parent.gameObject.SetActive(enable);

    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);

        entity?.ReciveKnockback(knockback, duration);
    }
    private Vector2 CalculateKnockback(float damage,Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyDamageKnockback : onDamageKnockback ;

        knockback.x=knockback.x*direction;  

        return knockback;
    }
    private float CalculateDuration(float damage)
    {
        return IsHeavyDamage(damage)? heacyKnockbackDuration:knockbackDuration;
    }
    private bool IsHeavyDamage(float damage) 
    {
        if(entityStats==null)
            return false;
        else
            return damage/entityStats.GetMaxHealth()>heavyDamageThreshold; 
    }

}
