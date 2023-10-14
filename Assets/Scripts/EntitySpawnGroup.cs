using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Game/EntitySpawnGroupd")]
public class EntitySpawnGroup : ScriptableObject
{
    public Entity[] entities;
}
