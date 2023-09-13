using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{

    [Header("Consumable Settings")]
    public float Feed = 0f;
    public float Heal = 0f;

    public TempraryEffects[] consumptionEffects;

    public override void onUse(Inventory inventory, int index)
    {
        base.onUse(inventory, index);

        if (inventory.slots[index].amount < 0)
        {
            inventory.slots[index] = null;
            return;
        }
        else
        {
            inventory.slots[index].amount--;

            foreach (TempraryEffects effect in consumptionEffects)
            {
                
            }
        }
    }

    [System.Serializable]
    public struct TempraryEffects
    {
        public float Time;

        public Statistics effectStatistics;
    }

    IEnumerator activateEffect(TempraryEffects effect)
    {
        Player.player.EffectStatistics.Add(effect.effectStatistics);
        yield return new WaitForSeconds(effect.Time);
        Player.player.EffectStatistics.Remove(effect.effectStatistics);
    }
}
