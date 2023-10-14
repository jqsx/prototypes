using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Game/SwordNames")]
public class SwordConfig : ScriptableObject
{
    public string[] attributes;

    public string getRandomAttribute()
    {
        if (attributes.Length == 0) return null;
        return attributes[Random.Range(0, attributes.Length)];
    }
}
