using System;
using System.Collections.Generic;
using UnityEngine;

//****************************************
//******DESCRIPCION BUGS SOLUCIONADOS*****
//****************************************
//BUG1 :Clientes avanzan a la siguiente posicion
//      pero se solapan en la posicion 1 y no
//      aparece C3. Eliminacion deinstanciaciones 
//      de npc para evitar solapamiento de npc
//SCRIPTS: Customer y CustomerQueue 

public enum CustomerState
{
    Waiting,
    InTreatment,
    Finished
}
[Serializable]
public class Customer : MonoBehaviour
{
    [SerializeField] private CustomerData data;

    public CustomerData Data
{
    get 
    { 
        return data; 
    }
}
    [SerializeField] private CharacterStats stats;
    public GameObject CustomerModel;

    private readonly List<Drug> receivedDrugs = new();

    private float satisfaction;
    private bool finishedTreatment;

    public void Awake()
    {
        //**********SCM-INI-BUG1 *************
        //CustomerModel = Instantiate(data.AvatarPrefab);
        // Si la referencia data o AvatarPrefab están asignados, instancia
        if (data != null && data.AvatarPrefab != null)
        {
            CustomerModel = Instantiate(data.AvatarPrefab, transform);
        }
        else
        {
            // Si el personaje ya tiene su gráfico configurado en la escena
            CustomerModel = gameObject;
        }
        //**********SCM-FIN-BUG1 *************
    }
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