using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDrops : MonoBehaviour
{
    public ItemSpawnChance[] itemDrops;

    public ItemStack[] executeItemDrop()
    {
        if (itemDrops.Length == 0) return new ItemStack[0];
        List<ItemStack> drops = new List<ItemStack>();
        for (int j = 0; j < itemDrops.Length; j++)
        {
            if (Random.Range(0f, 1f) < itemDrops[j].chance)
            {
                ItemStack bobux = new ItemStack(itemDrops[j].item, Mathf.Clamp(Random.Range(itemDrops[j].amount.x, itemDrops[j].amount.y), 1, itemDrops[j].item.maxStackSize));
                drops.Add(bobux);
            }
        }
        return drops.ToArray();
    }
}

[System.Serializable]
public class ItemSpawnChance
{
    public Item item;
    public float chance;
    public Vector2Int amount;
}