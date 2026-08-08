using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "Game/Customer")]
public class CustomerData : ScriptableObject
{
    [Header("General")]
    public string CustomerName;

    [TextArea]
    public string Description;

    [Header("Avatar")]
    public GameObject AvatarPrefab;

    [Header("Stats")]
    public List<Stat> Stats = new();
    [Header("StatObjective")]
    public List<StatObjective> Objectives  = new();
    
    [Header("Treatment")]
    [Range(1, 3)]
    public int MaxDrugs;
}