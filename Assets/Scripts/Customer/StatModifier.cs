using System;
using UnityEngine;

[Serializable]
public enum ModifierType
{
    Add,
    Multiply,
    Set
}
[Serializable]
public class StatModifier
{
    public StatType Stat;
    public ModifierType ModifierType;

    [Tooltip("Positive or negative value.")]
    public float Amount;
}
