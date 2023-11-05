using JQUI;
using Quests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class QuestMenuUI : MonoBehaviour
{
    public static bool isOpen = false;
    public TMP_Text QuestInfoText;
    public static QuestMenuUI instance;
    private QuestMaster current;
    public GameObject UIParent;

    public Transform QuestListParent;
    public QuestDisplay prefab;

    public Sprite slayerIcon;
    public Sprite collectIcon;

    private float actionDelay = 0f;

    private Quest currentlySelected;

    public TMP_Text CurrentQuestInfo;

    public GameObject QuestBoardParent;
    public GameObject ActiveQuestParent;

    void Start()
    {
        instance = this;
        UIParent.SetActive(false);
        isOpen = false;
    }

    void Update()
    {
        if (isOpen && InventoryController.isOpen) close();
        else if (isOpen && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Tab))) close();

        if (isOpen && current)
        {
            if (Vector2.Distance(Camera.main.transform.position, current.transform.position) > 2.5f)
            {
                close();
            }
        }
    }

    public void open(QuestMaster questMaster)
    {
        if ( !isOpen )
        {
            if (actionDelay > Time.time) return;
            actionDelay = Time.time + 0.1f;
            isOpen = true;
            current = questMaster;
            currentlySelected = null;

            updateView();

            UIParent.SetActive(true);
        }
    }

    public void show(Quest quest)
    {
        QuestInfoText.text = quest.questName;

        QuestInfoText.text += "\n\n";

        if (quest is SlayerQuest slayer) 
        { 
            foreach (string entity in slayer.SlayerEntityObjective.Keys)
            {
                slayer.SlayerEntityObjective.TryGetValue(entity, out int a);
                QuestInfoText.text += "- " + entity + " x" + a + "\n";
            }

            QuestInfoText.text += "\nFor:\n";

            foreach (ItemStack reward in slayer.rewards)
            {
                QuestInfoText.text += reward.item.name + " x" + reward.amount + "\n";
            }
        }
        else if (quest is CollectionQuest collect)
        {
            foreach (ItemStack reward in collect.itemsToCollect)
            {
                QuestInfoText.text += reward.item.name + " x" + reward.amount + "\n";
            }

            QuestInfoText.text += "\nFor:\n";

            foreach (ItemStack reward in collect.reward)
            {
                QuestInfoText.text += reward.item.name + " x" + reward.amount + "\n";
            }
        }

        currentlySelected = quest;
    }

    public void SelectQuest()
    {
        QuestManager.CurrentPlayerQuest = currentlySelected;
        updateView();
    }

    public void close()
    {
        if ( isOpen )
        {
            if (actionDelay > Time.time) return;
            actionDelay = Time.time + 0.1f;
            isOpen = false;
            current = null;
            currentlySelected = null;

            UIParent.SetActive(false);
        }
    }

    public void CancelQuest()
    {
        QuestManager.CurrentPlayerQuest = null;

        updateView();
    }

    private void updateView()
    {

        for (int i = QuestListParent.childCount - 1; i >= 0; i--)
        {
            Destroy(QuestListParent.GetChild(i).gameObject);
        }

        foreach (Quest quest in QuestManager.QuestList)
        {
            Sprite i = quest is SlayerQuest ? slayerIcon : collectIcon;
            QuestDisplay questDisplay = Instantiate(prefab, QuestListParent);
            questDisplay.init(quest, i);
        }

        if (QuestManager.CurrentPlayerQuest != null)
        {
            ActiveQuestParent.SetActive(true);
            QuestBoardParent.SetActive(false);

            Quest quest = QuestManager.CurrentPlayerQuest;

            CurrentQuestInfo.text = quest.questName;

            if (quest is SlayerQuest slayer)
            {
                foreach (string entity in slayer.SlayerEntityObjective.Keys)
                {
                    slayer.SlayerEntityObjective.TryGetValue(entity, out int a);
                    CurrentQuestInfo.text += "- " + entity + " x" + a + "\n";
                }

                CurrentQuestInfo.text += "\nFor:\n";

                foreach (ItemStack reward in slayer.rewards)
                {
                    CurrentQuestInfo.text += reward.item.name + " x" + reward.amount + "\n";
                }
            }
            else if (quest is CollectionQuest collect)
            {
                foreach (ItemStack reward in collect.itemsToCollect)
                {
                    CurrentQuestInfo.text += reward.item.name + " x" + reward.amount + "\n";
                }

                CurrentQuestInfo.text += "\nFor:\n";

                foreach (ItemStack reward in collect.reward)
                {
                    CurrentQuestInfo.text += reward.item.name + " x" + reward.amount + "\n";
                }
            }
        }
        else
        {
            ActiveQuestParent.SetActive(false);
            QuestBoardParent.SetActive(true);
        }
    }

    public void CompleteQuest()
    {
        Quest quest = QuestManager.CurrentPlayerQuest;

        if (quest != null)
        {
            if (quest is SlayerQuest slayer)
            {
                if (slayer.isComplete())
                {
                    QuestManager.QuestList.Remove(quest);

                    foreach (ItemStack reward in slayer.rewards)
                    {
                        GAMEINITIALIZER.SpawnItem(reward, current.transform.position);
                    }

                    QuestCompleted();
                }
            }
            else if (quest is CollectionQuest collection)
            {
                if (collection.isComplete())
                {
                    QuestManager.QuestList.Remove(quest);

                    foreach (ItemStack reward in collection.reward)
                    {
                        GAMEINITIALIZER.SpawnItem(reward, current.transform.position);
                    }

                    QuestCompleted();
                }
            }
        }
    }

    private void QuestCompleted()
    {
        currentlySelected = null;
        QuestManager.CurrentPlayerQuest = null;
        updateView();
    }
}
