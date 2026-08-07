
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    [SerializeField] 
    private CustomerData customerData;
    [SerializeField]
    private List<Stat> initialStats = new();

    private Dictionary<StatType, Stat> stats = new();

    private void Awake()
    {
        if (customerData == null)
            return;

        foreach (Stat statData in customerData.Stats)
        {
            stats.Add(statData.Type, new Stat
            {
                Type = statData.Type,
                Value = statData.Value
            });
        }
    }
    public float GetStat(StatType type)
    {
        if (stats.TryGetValue(type, out Stat stat))
            return stat.Value;

        return 0;
    }

    public void SetStat(StatType type, float value)
    {
        if (!stats.ContainsKey(type))
            return;

        stats[type].Value = Mathf.Clamp(value, 0, 100);
    }

    public void ModifyStat(StatType type, float amount)
    {
        if (!stats.ContainsKey(type))
            return;

        stats[type].Value = Mathf.Clamp(stats[type].Value + amount, 0, 100);
    }
    public void ApplyModifier(StatModifier modifier)
    {
        switch (modifier.ModifierType)
        {
            case ModifierType.Add:
                ModifyStat(modifier.Stat, modifier.Amount);
                break;

            case ModifierType.Set:
                SetStat(modifier.Stat, modifier.Amount);
                break;

            case ModifierType.Multiply:
                SetStat(modifier.Stat, GetStat(modifier.Stat) * modifier.Amount);
                break;
        }
    }
    public CustomerData Data => customerData;
}