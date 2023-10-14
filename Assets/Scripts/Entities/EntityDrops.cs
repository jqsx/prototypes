using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDrops : MonoBehaviour
{
    Entity entity;

    [HideInInspector]
    public List<ItemStack> droppedItems = new List<ItemStack>();
    public Vector2Int amountOfDroppedItemsRange = new Vector2Int(1, 2);
    public DropChance[] dropChances;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    public void onDeath()
    {
        int count = Random.Range(amountOfDroppedItemsRange.x, amountOfDroppedItemsRange.y);
        for (int i = 0; i < count; i++)
        {
            DropChance dropChance = dropChances[Random.Range(0, dropChances.Length - 1)];
            ItemStack itemstack = dropChance.getItem();
            if (itemstack != null) droppedItems.Add(itemstack);
        }
    }
}


[System.Serializable]
public class DropChance
{
    public float chance = 0f;
    public Vector2Int levelRange = new Vector2Int(1, 2);
    public Vector2Int amountRange = new Vector2Int(1, 2);
    public Item item;
    public Item.ItemType itemType;

    public bool isID = false;
    public int id = 0;

    public bool preset = false;

    public DropChance(float chance, Vector2Int levelRange, Item item)
    {
        this.chance = chance;
        this.levelRange = levelRange;
        this.item = item;

        preset = true;
    }

    public DropChance(float chance, Vector2Int levelRange, Item.ItemType itemType)
    {
        this.chance = chance;
        this.levelRange = levelRange;
        this.itemType = itemType;

        preset = false;
    }

    public ItemStack getItem()
    {
        if (isID)
        {
            int amount = Random.Range(amountRange.x, amountRange.y);
            return new ItemStack(GAMEINITIALIZER.getItem(id), amount);
        }
        else if (!preset)
        {
            if (itemType == Item.ItemType.Weapon) return getRandomWeapon();
        }
        else
        {
            int amount = Random.Range(amountRange.x, amountRange.y);
            return new ItemStack(item, amount);
        }
        

        return null;
    }

    private ItemStack getRandomWeapon()
    {
        if (Random.Range(0f, 1f) < chance)
        {
            int level = Random.Range(levelRange.x, levelRange.y);

            float damage = level * 2 + 1;

            float attackSpeed = Random.Range(0f, 3f) - 1f;

            WeaponItem weapon = new WeaponItem();

            weapon.itemType = Item.ItemType.Weapon;
            weapon.itemStatistics.Damage = damage;
            weapon.itemStatistics.AttackSpeed = attackSpeed;
            weapon.itemStatistics.Level = level;
            weapon.maxStackSize = 1;

            float index = 1f - (attackSpeed + 1f) / 3f;

            SpriteStorage.RegisterSprite data = SpriteStorage.instance.Weapons[(int)Mathf.Round(index * (SpriteStorage.instance.Weapons.Length - 1))];
            weapon.setName(GAMEINITIALIZER.instance.swordAttributes.getRandomAttribute() + " " + data.name).setDesc("Bing description bing");
            weapon.setHandleOffset(data.handOffset);
            weapon.setIcon(data.sprite);
            return new ItemStack(weapon, 1);
        }

        return null;
    }
}