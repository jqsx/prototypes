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
    public float Damage = 1f;
    public float MoveSpeed = 3f;
    public float AttackSpeed = 1f;
    public float MaxHealth = 20f;
    public float Health = 20f;
    public float Defence = 0f;
    /// <summary>
    /// 0.5 + RegenerationRate = Total
    /// </summary>
    public float RegenerationRate = 0f;
}