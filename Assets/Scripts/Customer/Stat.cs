

using System;
using UnityEngine;
[Serializable]

public enum StatType
{
    Energy,
    Concentration,
    Stress,
    Anxiety,
    Mood,
    Health,
    Tolerance,
    Willpower
}

[Serializable]
public class Stat
{
    public StatType Type;

    public int MinValue = 0;
    public int MaxValue = 100;

    [Range(0, 100)]
    public float Value = 50;
}