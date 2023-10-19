using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class GAMEINITIALIZER : MonoBehaviour
{
    public static int globalGameLevel = 1;

    public static GAMEINITIALIZER instance;
    private static List<Item> InitializedItems;
    public ItemRegistry itemRegistry = new ItemRegistry();

    [System.Serializable]
    public class ItemRegistry
    {
        public List<Item> registeredItems = new List<Item>();
        public List<ConsumableItem> registeredConsumableItems = new List<ConsumableItem>();
        public List<WeaponItem> registeredWeaponItems = new List<WeaponItem>();
    }

    [Header("Global Prefabs")]
    public ItemDrop itmdrop;
    public DamageIndicator dmgind;
    public static ItemDrop prefab_ItemDrop;
    public static DamageIndicator prefab_DamageIndicator;
    public Player player_prefab;

    public ParticleSystem bloodSplatter;
    public static ParticleSystem prefab_bloodSplatter;

    public SpriteStorage spriteStorage = new SpriteStorage();

    [Header("Global Settings")]
    public bool autoSpawnPlayer = true;
    [HideInInspector]
    public static Vector2 globalSeed;

    public SwordConfig swordAttributes;

    private void Start()
    {
        init();
    }

    public static void init()
    {
        globalSeed = new Vector2(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));

        if (roomGenerator.GEN != null)
        {
            roomGenerator.GEN.updateSeed();
            roomGenerator.GEN.generateRoom(null);
        }
    }

    private void Awake()
    {
        spriteStorage.init();
        Debug.Log("Initializing Game data");
        instance = this;
        if (InitializedItems == null)
        {
            List<Item> items = new List<Item>();
            items.InsertRange(0, itemRegistry.registeredItems);
            items.InsertRange(0, itemRegistry.registeredConsumableItems);
            items.InsertRange(0, itemRegistry.registeredWeaponItems);
            InitializedItems = items;
        }

        prefab_ItemDrop = itmdrop;
        prefab_DamageIndicator = dmgind;
        prefab_bloodSplatter = bloodSplatter;

        if (PlayerController.instance == null && autoSpawnPlayer)
        {
            Instantiate(player_prefab, transform.position, Quaternion.identity);
        }
        Debug.Log("Initialized Game data");
    }

    public void debug()
    {
        ItemDrop drop = SpawnItem(0, 1, Vector2.zero);
        Sprite a = drop.itemStack.item.icon;

        Statistics weaponStats = new Statistics();
        weaponStats.Level = 34;
        weaponStats.Damage = 35.2f;
        WeaponItem i = new WeaponItem();
        i.itemStatistics = weaponStats;
        i.icon = a;
        i.name = "aAAA";
        i.description = "aaa";
        i.itemType = Item.ItemType.Weapon;

        SpawnItem(new ItemStack(i, 1), Vector2.zero);
    }

    public static int InitializeItem(Item item)
    {
        InitializedItems.Add(item);
        return InitializedItems.Count - 1;
    }

    public static Item getItem(int id)
    {
        try
        {
            return InitializedItems[id];
        } catch
        {
            Debug.Log(new System.Exception("INVALID ITEM ID PROVIDED, COULDN'T RETRIEVE ITEM!"));
            return null;
        }
    }

    public static void SpawnBloodSplatter(Vector2 position)
    {
        instance.StartCoroutine(instance.DestroyObjectIn(Instantiate(prefab_bloodSplatter, position, Quaternion.identity).gameObject, prefab_bloodSplatter.main.duration));
    }

    public static ItemDrop SpawnItem(int id, int amount, Vector2 position)
    {
        ItemStack n = new ItemStack(getItem(id), amount);
        return Instantiate(prefab_ItemDrop, position, Quaternion.identity).AssignItem(n);
    }

    public static ItemDrop SpawnItem(int id, Vector2 position)
    {
        return SpawnItem(id, 1, position);
    }

    public static ItemDrop SpawnItem(ItemStack itemStack, Vector2 position)
    {
        return Instantiate(prefab_ItemDrop, position, Quaternion.identity).AssignItem(itemStack);
    }

    public static void spawnTextIndicator(float amount, Vector2 position)
    {
        Instantiate(prefab_DamageIndicator, position, Quaternion.identity).setup(amount);
    }

    public static void spawnTextIndicator(float amount, Vector2 position, Color color)
    {
        Instantiate(prefab_DamageIndicator, position, Quaternion.identity).setup(amount).setColor(color);
    }

    public static void spawnDamageIndicator(float amount, Vector2 position)
    {
        Instantiate(prefab_DamageIndicator, position, Quaternion.identity).setup(amount).parseDamageTextAsColors();
    }

    IEnumerator DestroyObjectIn(GameObject gm, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gm);
    }
}

[System.Serializable]
public class Item
{
    public string name;
    public Sprite icon;
    [TextArea]
    public string description;
    public ItemType itemType = ItemType.Material;
    public int maxStackSize = 1;
    public Statistics itemStatistics = new Statistics();

    public Item()
    {

    }

    public virtual void onUse(Inventory inventory, int index)
    {

    }

    public Item(Item item) : this(item.icon, item.name, item.description, item.itemStatistics)
    {

    }

    public Item(Sprite icon, string name, string desc, Statistics itemStatistics)
    {
        this.icon = icon;
        this.name = name;
        this.description = desc;
        this.itemStatistics = itemStatistics;
    }

    public Item setName(string name)
    {
        this.name = name;
        return this;
    }

    public Item setDesc(string description)
    {
        this.description = description;
        return this;
    }

    public Item setStats(Statistics stats)
    {
        this.itemStatistics = stats;
        return this;
    }

    public Item setIcon(Sprite icon)
    {
        this.icon = icon;
        return this;
    }

    public enum ItemType
    {
        Weapon, 
        Food, 
        Armor, 
        Material
    }
}

[System.Serializable]
public class GeneratedRandomItem : Item
{
    public GeneratedRandomItem(int level)
    {
        // generate random item here
    }
}

[System.Serializable]
public class ItemStack
{
    public Item item;
    public int amount = 1;

    public ItemStack(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public ItemStack addItem(ItemStack stack)
    {
        if (stack.item.name != item.name) return stack;
        int total = stack.amount + amount;
        if (total > item.maxStackSize)
        {
            amount = item.maxStackSize;
            stack.amount = total - item.maxStackSize;
            return stack;
        }
        else
        {
            amount = total;
            return null;
        }
    }
}

[System.Serializable]
public class SpriteStorage
{
    public static SpriteStorage instance;
    public static Dictionary<string, Sprite> weaponSprites = new Dictionary<string, Sprite>();
    public static Dictionary<string, Sprite> armorSprites = new Dictionary<string, Sprite>();

    public RegisterSprite[] Weapons;
    public RegisterSprite[] Armor;

    [System.Serializable]
    public class RegisterSprite
    {
        public string name;
        public string displayName;
        public Sprite sprite;
        public Vector2 handOffset = Vector2.zero;
    }

    public void init()
    {
        instance = this;
        foreach(RegisterSprite registerSprite in Weapons)
        {
            if (!weaponSprites.ContainsKey(registerSprite.name)) weaponSprites.Add(registerSprite.name, registerSprite.sprite);
        }

        foreach (RegisterSprite registerSprite in Armor)
        {
            if (!armorSprites.ContainsKey(registerSprite.name)) armorSprites.Add(registerSprite.name, registerSprite.sprite);
        }

        Debug.Log("Initialized Sprite Storage");
    }
}