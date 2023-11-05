using Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    private static bool isStarted = false;
    public static Quest CurrentPlayerQuest = null;

    public static List<Quest> QuestList = new List<Quest>();

    private static QuestEntityData entityData;
    private static QuestItemData itemData;

    public static Dictionary<string, int> entityKills = new Dictionary<string, int>();

    public static void EntityHasBeenKilled(Entity entity)
    {
        if (!entityKills.ContainsKey(entity.EntityName))
        {
            entityKills.Add(entity.EntityName, 1);
        }
        else
        {
            entityKills.TryGetValue(entity.EntityName, out int a);
            entityKills.Remove(entity.EntityName);
            entityKills.Add(entity.EntityName, a + 1);
        }
    }

    public static void setNewQuest(Quest quest)
    {
        entityKills.Clear();
        CurrentPlayerQuest = quest;
    }

    public static void QuestUpdate()
    {
        if (QuestList.Count < 3)
        {
            GenerateQuests();
        }
    }

    public static void GenerateQuests()
    {
        int count = Random.Range(4, 14);
        for (int i = 0; i < count; i++)
        {
            if (Random.Range(0f, 1f) < 0.5f)
            {
                QuestList.Add(GenerateSlayerQuest());
            }
            else
            {
                QuestList.Add(GenerateCollectionQuest());
            }
        }
    }

    public static SlayerQuest GenerateSlayerQuest()
    {
        string[] entities = new string[1];
        Entity entity = entityData.availableEntities[Random.Range(0, entityData.availableEntities.Length)];
        entities[0] = entity.EntityName;
        int[] amount = new int[1];
        amount[0] = Random.Range(8, 60);

        ItemStack[] rewards = new ItemStack[Random.Range(1, 4)];
        for (int i = 0; i < rewards.Length; i++)
        {
            rewards[i] = new ItemStack(itemData.Currency, Random.Range(3, 10));
        }

        return new SlayerQuest(entities, amount, rewards);
    }

    public static CollectionQuest GenerateCollectionQuest()
    {
        ItemStack[] toCollect = new ItemStack[Random.Range(1, 6)];
        ItemStack[] reward = new ItemStack[Random.Range(1, 4)];

        for(int i = 0; i < toCollect.Length; i++)
        {
            toCollect[i] = new ItemStack(itemData.availableItems[Random.Range(0, itemData.availableItems.Length)], Random.Range(1, 15));
        }

        for(int i = 0; i < reward.Length; i++)
        {
            reward[i] = new ItemStack(itemData.Currency, Random.Range(3, 10));
        }
        
        return new CollectionQuest(toCollect, reward);
    }

    public static void StartQuestManager()
    {
        if ( !isStarted )
        {
            isStarted = true;

            entityData = UIController.instance.questEntityData;
            itemData = UIController.instance.questItemData;

            GenerateQuests();
        }
    }
}