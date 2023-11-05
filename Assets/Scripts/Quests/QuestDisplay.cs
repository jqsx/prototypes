using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplay : MonoBehaviour
{
    public Image image;
    public TMP_Text infoText;

    private Quest quest;

    public void init(Quest quest, Sprite icon)
    {
        image.sprite = icon;
        infoText.text = quest.questName;
        this.quest = quest;

        GetComponent<Button>().onClick.AddListener(() => { QuestMenuUI.instance.show(quest); });
    }
}
