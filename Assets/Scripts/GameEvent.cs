using System.Collections.Generic;

public class GameEvent
{
    bool isCanceled = false;

    public void setCancelled(bool cancelled)
    {
        isCanceled = cancelled;
    }

    public bool isCancelled()
    {
        return isCanceled;
    }
}

public class EntityDamageEvent : GameEvent
{
    public float amount = 0f;
    public Entity from;
    public Entity.DamageCause damageCause = Entity.DamageCause.None;

    public EntityDamageEvent(float amount, Entity from, Entity.DamageCause damageCause)
    {
        this.amount = amount;
        this.from = from;
        this.damageCause = damageCause;
    }
}

public class EntityDeathEvent : GameEvent
{
    public readonly float amount = 0f;
    public readonly Entity from;
    public readonly Entity.DamageCause damageCause = Entity.DamageCause.None;
    public List<ItemStack> eventDrops;
    private EntityDrops drops;

    public EntityDeathEvent(float amount, Entity from, Entity.DamageCause damageCause, EntityDrops drops)
    {
        this.amount = amount;
        this.from = from;
        this.damageCause = damageCause;
        if (drops != null)
        {
            drops.onDeath();

            this.drops = drops;
            this.eventDrops = drops.droppedItems;
        }
    }

    public void EndEvent()
    {
        if (drops != null)
        {
            drops.droppedItems = eventDrops;
        }
    }
}