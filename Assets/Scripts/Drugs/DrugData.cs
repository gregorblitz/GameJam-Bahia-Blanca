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

    [Header("Stat Effects")]
    public List<StatModifier> StatModifiers = new();

    [Header("Components (Future)")]
    public List<ComponentRequirement> RequiredComponents = new();

    [Serializable]
    public class StatModifier
    {
        public StatType Stat;

        [Tooltip("Positive or negative value.")]
        public int Amount;
    }

    [Serializable]
    public class ComponentRequirement
    {
        public string ComponentId;

        [Min(1)]
        public int Amount = 1;
    }
}