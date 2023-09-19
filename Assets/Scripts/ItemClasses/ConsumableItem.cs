using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class ConsumableItem : Item
{

    [Header("Consumable Settings")]
    public float Feed = 0f;
    public float Heal = 0f;

    public TempraryEffects[] consumptionEffects;

    public override void onUse(Inventory inventory, int index)
    {
        base.onUse(inventory, index);

        if (inventory.slots[index].amount <= 0)
        {
            inventory.slots[index] = null;
            return;
        }
        else
        {
            inventory.slots[index].amount--;

            Player.player.Health = Mathf.Clamp(Player.player.Health + Heal, 0, Player.player.getTotal().MaxHealth);

            foreach (TempraryEffects effect in consumptionEffects)
            {
                JQUI.InventoryController.ActivateEffect(effect);
            }

            if (inventory.slots[index].amount <= 0)
            {
                inventory.slots[index] = null;
            }
        }
    }

    [System.Serializable]
    public struct TempraryEffects
    {
        public float Time;

        public Statistics effectStatistics;
    }
}
