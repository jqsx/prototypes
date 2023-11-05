using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Quest/EntityData")]
public class QuestEntityData : ScriptableObject
{
    public Entity[] availableEntities;
}
