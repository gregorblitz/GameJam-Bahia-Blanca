using UnityEngine;

public class Drug : MonoBehaviour
{
    [SerializeField] private DrugData data;

    public DrugData Data => data;

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