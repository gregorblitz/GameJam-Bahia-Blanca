using System.Collections.Generic;
using UnityEngine;


public enum CustomerState
{
    Waiting,
    InTreatment,
    Finished
}

public class Customer : MonoBehaviour
{
    [SerializeField] private CustomerData data;
    [SerializeField] private CharacterStats stats;

    private readonly List<Drug> receivedDrugs = new();

    private float satisfaction;
    private bool finishedTreatment;

    public bool ReceiveDrug(Drug drug)
    {
        if (finishedTreatment)
            return false;

        if (receivedDrugs.Count >= data.MaxDrugs)
            return false;

        receivedDrugs.Add(drug);

        drug.Apply(stats);

        return true;
    }
    public TreatmentResult Evaluate(float purchasePrice)
    {
        float objectiveScore = EvaluateObjectives();
        float generalScore = EvaluateGeneralStats();

        // 70% objetivos, 30% estado general
        float satisfaction = (objectiveScore * 0.70f) + (generalScore * 0.30f);

        satisfaction = Mathf.Clamp(satisfaction, 0f, 100f);

        bool success = objectiveScore >= 100f;

        float tip = purchasePrice * 0.30f * (satisfaction / 100f);

        return new TreatmentResult
        {
            Satisfaction = satisfaction,
            Tip = tip,
            Success = success
        };
    }

    private float EvaluateObjectives()
    {
        if (data.Objectives == null || data.Objectives.Count == 0)
            return 100f;

        float totalScore = 0f;

        foreach (var objective in data.Objectives)
        {
            float currentValue = stats.GetStat(objective.Stat);

            if (currentValue >= objective.MinValue &&
                currentValue <= objective.MaxValue)
            {
                totalScore += 100f;
            }
        }

        return totalScore / data.Objectives.Count;
    }


    private float EvaluateGeneralStats()
    {
        if (data.Stats == null || data.Stats.Count == 0)
            return 100f;

        float totalScore = 0f;

        foreach (var initialStat in data.Stats)
        {
            float initialValue = initialStat.Value;
            float currentValue = stats.GetStat(initialStat.Type);

            float difference = currentValue - initialValue;

            float statScore;

            if (difference > 0)
            {
                statScore = 100f;
            }
            else if (difference == 0)
            {
                statScore = 100f;
            }
            else
            {
                // Cada punto perdido reduce la puntuación.
                statScore = 100f + difference;
            }

            statScore = Mathf.Clamp(statScore, 0f, 100f);

            totalScore += statScore;
        }

        return totalScore / data.Stats.Count;
    }
}