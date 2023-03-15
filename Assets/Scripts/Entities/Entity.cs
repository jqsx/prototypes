using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("ENTITY PARAMETERS")]
    public float Health = 20f;
    public Statistics EntityStatistics = new Statistics();

    private float regenDelay = 0f;
    private float lastRegenTick = 0f;

    private void Update()
    {
        if (lastRegenTick < Time.time && regenDelay < Time.time)
        {
            lastRegenTick = Time.time + 0.5f * (1f - EntityStatistics.RegenerationRate);
            regenerationTick();
        }
    }

    private void regenerationTick()
    {
        Health = Mathf.Clamp(Health + 0.5f, 0f, EntityStatistics.MaxHealth);
    }

    public void damage(float amount, Entity from, DamageCause damageCause)
    {
        EntityDamageEvent e = new EntityDamageEvent(amount, from, damageCause);
        onDamage(e);
        if (e.isCancelled()) return;
        if (from != null)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb) rb.velocity += from.GetComponent<Rigidbody2D>().velocity / 2f;
        }
        Health = Mathf.Clamp(Health - amount, 0, EntityStatistics.MaxHealth);
        GAMEINITIALIZER.spawnTextIndicator(amount, transform.position);
        if (Health == 0)
        {
            death();
        }
        regenDelay = Time.time + 2f;
    }

    private void death()
    {
        EntityDeathEvent e = new EntityDeathEvent();
        onDeath(e);
        if (e.isCancelled()) return;

        for(int i = 0; i < 5; i++)
        {
            if (Random.Range(0f, 1f) > 0.2f * i)
            {
                GAMEINITIALIZER.SpawnItem(2, transform.position).GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            }
        }

        Destroy(gameObject);
    }

    public virtual void onDeath(EntityDeathEvent e)
    {

    }

    public void damage(float amount, Entity from)
    {
        damage(amount, from, DamageCause.None);
    }

    public virtual void onDamage(EntityDamageEvent e)
    {
        
    }

    public enum DamageCause
    {
        Attack, Projectile, Magic, Evironment, Unknown, None
    }

    public virtual void onAttack()
    {

    }

    public void Attack(Vector2 direction)
    {
        Collider2D[] all = Physics2D.OverlapBoxAll((Vector2)transform.position - direction, new Vector2(1, 2), Mathf.Atan2(direction.x, direction.y));
        foreach(Collider2D col in all)
        {
            if (col.transform == transform) continue;
            Entity entity = col.GetComponent<Entity>();
            if (entity)
            {
                entity.damage(EntityStatistics.Damage, this);
            }
        }
    }
}

[System.Serializable]
public class Statistics
{
    public int Level = 1;
    public float XP = 0f;
    public float Damage = 1f;
    public float MoveSpeed = 3f;
    public float AttackSpeed = 1f;
    public float MaxHealth = 20f;
    public float Defence = 0f;
    /// <summary>
    /// 0.5 * (1f - RegenerationRate) = Regen tick health basically
    /// </summary>
    public float RegenerationRate = 0f;
}