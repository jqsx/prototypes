using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("ENTITY PARAMETERS")]
    public Statistics EntityStatistics = new Statistics();

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}

[System.Serializable]
public class Statistics
{
    public int Level = 1;
    public float XP = 0f;
    public float Damage = 0f;
    public float MoveSpeed = 0f;
    public float AttackSpeed = 0f;
}