﻿public class GameEvent
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

}