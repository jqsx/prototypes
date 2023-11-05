using JQUI;
using System;
using System.Collections.Generic;
using System.Xml;
using static UnityEditor.Progress;

[Serializable]
public class Quest
{
    public string questName = "QuestName";
    public virtual bool isComplete()
    {
        return false;
    }
}

namespace Quests
{
    [Serializable]
    public class CollectionQuest : Quest
    {
        public static string QuestDisplay = "[ <color=lime>Collection Quest</color> ]";
        public ItemStack[] itemsToCollect;

        public ItemStack[] reward;

        public CollectionQuest(ItemStack[] itemsToCollect, ItemStack[] reward)
        {
            this.itemsToCollect = itemsToCollect;
            this.reward = reward;
        }

        public void autoGenQuestName()
        {
            string toCollect = "";
            for (int i = 0; i < itemsToCollect.Length; i++)
            {
                toCollect += itemsToCollect[i].item.name + ", ";
            }
            string rew = "";
            for (int i = 0; i < reward.Length; i++)
            {
                rew += reward[i].item.name + (i < reward.Length - 1 ? ", " : "");
            }
            questName = "Quest to collect: <color=cyan>" + toCollect + "</color>for <color=cyan>" + rew;
        }

        public override bool isComplete()
        {
            Dictionary<string, int> items = new Dictionary<string, int>();

            foreach (ItemStack item in InventoryController.inventory.slots)
            {
                if (item != null)
                {
                    if (!items.ContainsKey(item.item.name))
                    {
                        items.Add(item.item.name, item.amount);
                    }
                    else
                    {
                        items.TryGetValue(item.item.name, out int a);
                        items.Remove(item.item.name);
                        items.Add(item.item.name, a + item.amount);
                    }
                }
            }

            foreach (ItemStack item in itemsToCollect)
            {
                if (!items.ContainsKey(item.item.name)) return false;
                items.TryGetValue(item.item.name, out int a);
                if (a < item.amount) return false;
            }
            return true;
        }
    }

    public class SlayerQuest : Quest
    {
        public static string QuestDisplay = "[ <color=red>SlayerQuest</color> ]";
        public Dictionary<string, int> SlayerEntityObjective = new Dictionary<string, int>();

        public ItemStack[] rewards;

        public SlayerQuest(string[] RegisteredEntityNames, int[] expectedSlayCount, ItemStack[] rewards)
        {
            if (RegisteredEntityNames.Length != expectedSlayCount.Length) throw new Exception("Invalid entity and slay count quest");
            for (int i = 0; i < RegisteredEntityNames.Length; i++)
            {
                SlayerEntityObjective.Add(RegisteredEntityNames[i], expectedSlayCount[i]);
            }

            this.rewards = rewards;
        }

        public override bool isComplete()
        {
            foreach (string key in SlayerEntityObjective.Keys)
            {
                int a = SlayerEntityObjective[key];

                if (!QuestManager.entityKills.ContainsKey(key)) return false;
                int k = QuestManager.entityKills[key];
                if (k < a) return false;
            }
            return true;
        }
    }

    
}