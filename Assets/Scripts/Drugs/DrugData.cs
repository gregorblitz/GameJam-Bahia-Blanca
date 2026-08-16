using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drug", menuName = "Game/Drug")]
public class DrugData : ScriptableObject
{
    [Header("General")]
    public string DrugName;
    public string CodeName;

    [TextArea]
    public string Description;

    [Header("Duration")]
    [Min(0)]
    public float DurationHours = 24f;
    
    [Header("Costo")]
    [Min(0)]
    public float Cost = 10f;

    [Header("Stat Effects")]
    public List<StatModifier> StatModifiers = new();

    [Header("Components (Future)")]
    public List<ComponentRequirement> RequiredComponents = new();
}