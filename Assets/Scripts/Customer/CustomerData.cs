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
    public List<StatData> Stats = new();

    [Serializable]
    public class StatData
    {
        public StatType Type;

        [Range(0, 100)]
        public int Value = 50;
    }
}