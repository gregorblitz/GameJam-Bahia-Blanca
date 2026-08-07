using System;
using UnityEngine;

[Serializable]
public class StatObjective
{
    public StatType Stat;

    [Range(0,100)]
    public int MinValue;

    [Range(0,100)]
    public int MaxValue;

    public bool Required = true;
}