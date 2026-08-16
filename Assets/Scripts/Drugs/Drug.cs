using UnityEngine;
using System;
[Serializable]
public class Drug : MonoBehaviour
{
  
    [SerializeField] private DrugData data;

    public DrugData Data => data;

    public void Initialize(DrugData drugData)
    {
        data = drugData;
    }

    public void Apply(CharacterStats target)
    {
        if (target == null || data == null)
            return;

        foreach (var modifier in data.StatModifiers)
        {
            target.ApplyModifier(modifier);
        }
    }
}