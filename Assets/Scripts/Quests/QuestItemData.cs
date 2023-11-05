using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Quest/ItemData")]
public class QuestItemData : ScriptableObject
{
    public Item Currency;
    public Item[] availableItems;
}
