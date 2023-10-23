using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smily : BasicAI
{
    public override void EntityAwake()
    {
        Statistics statistics = new Statistics();
        statistics.MoveSpeed = 5 * Mathf.Clamp(GAMEINITIALIZER.globalGameLevel / 5, 1, 10);
        statistics.Damage = GAMEINITIALIZER.globalGameLevel * 1.2f;
        statistics.AttackSpeed = 1;
        statistics.MaxHealth = GAMEINITIALIZER.globalGameLevel * 1.5f;
        Health = statistics.MaxHealth;
        EntityStatistics = statistics;
        statistics.Level = GAMEINITIALIZER.globalGameLevel;

        EntityDrops drops = GetComponent<EntityDrops>();
        if (drops != null)
        {
            foreach (DropChance chance in drops.dropChances)
            {
                if (!chance.preset) chance.levelRange = new Vector2Int(Mathf.Clamp(GAMEINITIALIZER.globalGameLevel - 5, 1, GAMEINITIALIZER.globalGameLevel), GAMEINITIALIZER.globalGameLevel + 2);
            }
        }
    }

    public override void onDeath(EntityDeathEvent e)
    {
        Item randomItem = GAMEINITIALIZER.getItem(Random.Range(0, 10));

        if (randomItem != null)
        {
            GAMEINITIALIZER.SpawnItem(new ItemStack(randomItem, Random.Range(1, randomItem.maxStackSize)), transform.position);
        }
    }

    private void Update()
    {
        BasicAIUpdate();
    }
}
