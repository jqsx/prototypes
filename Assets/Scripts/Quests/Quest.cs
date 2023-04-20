using System;
using System.Collections.Generic;

[Serializable]
public class Quest
{
    public string questName = "QuestName";
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
    }

    
}