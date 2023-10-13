using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("ENTITY PARAMETERS")]
    public float Health = 20f;
    public Statistics EntityStatistics = new Statistics();
    public Statistics EquipmentStatistics = new Statistics();
    public Statistics EffectStatistics = new Statistics();
    public GameObject HandPivot;
    [HideInInspector]
    public GameObject currentlySelectedVisual;

    private float regenDelay = 0f;
    private float lastRegenTick = 0f;

    public Faction faction = Faction.Passive;

    void Update()
    {
        regenCycle();
    }

    public void regenCycle()
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

        GAMEINITIALIZER.SpawnBloodSplatter(transform.position);

        Health = Mathf.Clamp(Health - amount, 0, EntityStatistics.MaxHealth);
        GAMEINITIALIZER.spawnDamageIndicator(amount, transform.position);
        if (Health == 0)
        {
            death(e);
        }
        regenDelay = Time.time + 2f;
    }

    private void death(EntityDamageEvent de)
    {

        ItemStack[] _droppedItems = new ItemStack[0];
        EntityDrops drops;
        if (drops = GetComponent<EntityDrops>())
        {
            //_droppedItems = drops.executeItemDrop();
            drops.onDeath();
            _droppedItems = drops.droppedItems.ToArray();
        }

        EntityDeathEvent e = new EntityDeathEvent(de.amount, de.from, de.damageCause, _droppedItems);

        onDeath(e);
        if (e.isCancelled()) return;

        foreach(ItemStack itemstack in _droppedItems)
        {
            GAMEINITIALIZER.SpawnItem(itemstack, transform.position).GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
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

    public void Attack(Vector2 direction, bool isCrit)
    {
        Collider2D[] all = Physics2D.OverlapBoxAll((Vector2)transform.position - direction, new Vector2(1, 2), Mathf.Atan2(direction.x, direction.y));
        foreach(Collider2D col in all)
        {
            if (col.transform == transform) continue;
            Entity entity = col.GetComponent<Entity>();
            if (entity)
            {
                entity.damage((EntityStatistics.Damage + EquipmentStatistics.Damage) * (1.5f + EntityStatistics.CritDamage + EquipmentStatistics.CritDamage), this);
            }
        }
    }

    public Statistics getTotal()
    {
        Statistics stats = new Statistics();
        stats.Add(EntityStatistics);
        stats.Add(EquipmentStatistics);
        stats.Add(EffectStatistics);

        return stats;
    }

    public void setTool(ItemStack itemStack)
    {
        if (currentlySelectedVisual != null)
            Destroy(currentlySelectedVisual);
        if (itemStack == null) return;
        GameObject tool = new GameObject();
        tool.transform.parent = HandPivot.transform;
        tool.transform.localPosition = Vector3.zero;
        tool.transform.localRotation = Quaternion.Euler(0, 0, 0);
        tool.AddComponent<SpriteRenderer>().sprite = itemStack.item.icon;
        
        if (itemStack.item is ToolItem)
        {
            ToolItem toolitem = (ToolItem)itemStack.item;
            tool.transform.localPosition = toolitem.handleOffset;
        }

        currentlySelectedVisual = tool;
    }
}

[System.Serializable]
public class Statistics
{
    public int Level = 1;
    public float XP = 0f;
    public float Damage = 0f;
    public float CritDamage = 0f;
    public float CritChance = 0f;
    public float MoveSpeed = 0f;
    public float AttackSpeed = 0f;
    public float MaxHealth = 0f;
    public float Defence = 0f;
    public float KnockBackResistance = 0f;
    /// <summary>
    /// 0.5 * (1f - RegenerationRate) = Regen tick health basically
    /// </summary>
    public float RegenerationRate = 0f;

    public void Add(Statistics other)
    {
        Damage += other.Damage;
        CritDamage += other.CritDamage;
        CritChance += other.CritChance;
        MoveSpeed += other.MoveSpeed;
        AttackSpeed += other.AttackSpeed;
        MaxHealth += other.MaxHealth;
        Defence += other.Defence;
        KnockBackResistance += other.KnockBackResistance;
        RegenerationRate += other.RegenerationRate;
}

    public void Remove(Statistics other)
    {
        Damage -= other.Damage;
        CritDamage -= other.CritDamage;
        CritChance -= other.CritChance;
        MoveSpeed -= other.MoveSpeed;
        AttackSpeed -= other.AttackSpeed;
        MaxHealth -= other.MaxHealth;
        Defence -= other.Defence;
        KnockBackResistance -= other.KnockBackResistance;
        RegenerationRate -= other.RegenerationRate;
    }

    public string toString()
    {
        string text = "";
        text += Level + "L\n";
        if (Damage != 0) text += "Damage: " + Damage + "\n";
        if (CritDamage != 0) text += "Crit.Damage: " + CritDamage + "%\n";
        if (CritChance != 0) text += "Crit.Chance: " + CritChance + "%\n";
        if (MoveSpeed != 0) text += "Speed: " + MoveSpeed + "\n";
        if (AttackSpeed != 0) text += "Attack Speed: " + AttackSpeed + "\n";
        if (MaxHealth != 0) text += "Health: " + MaxHealth + "\n";
        if (Defence != 0) text += "Defence: " + Defence;
        if (KnockBackResistance != 0) text += "Knockback Resistance: " + KnockBackResistance + "%\n";

        return text;
    }
}

public enum Faction
{
    Passive, Neutral, Aggressive
}