using UnityEngine;
using System;

[Serializable]
public class ComponentRequirement
{
    public string ComponentId;

    [Min(1)]
    public int Amount = 1;
}