using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CustomerQueue customerQueue;
    [SerializeField] private PlayerHand playerHand;

    [Header("Treatment Area")]
    [SerializeField] private DrugSlot[] treatmentSlots;

    [Header("Treatment")]
    [SerializeField] private float precioBaseTratamiento = 50f;
    [SerializeField] private float delayBeforeEvaluation = 1.5f;
    [SerializeField] private float delayBeforeNextCustomer = 2f;

    private bool processing;

    public bool IsProcessing => processing;

    public void ProcessPurchase()
    {
        if (processing)
            return;

        if (customerQueue == null)
        {
            Debug.LogWarning("CashRegister: CustomerQueue no asignado.");
            return;
        }

        Customer customer = customerQueue.CurrentCustomer;

        if (customer == null)
        {
            Debug.LogWarning("CashRegister: No hay cliente siendo atendido.");
            return;
        }

        if (playerHand != null && !playerHand.IsEmpty)
        {
            Debug.LogWarning(
                "CashRegister: El jugador todavía tiene un medicamento en la mano."
            );
            return;
        }

        List<Drug> drugs = GetDrugsFromTreatmentSlots();

        if (drugs.Count == 0)
        {
            Debug.LogWarning(
                "CashRegister: No hay medicamentos preparados."
            );
            return;
        }

        StartCoroutine(ProcessPurchaseCoroutine(customer, drugs));
    }

    private IEnumerator ProcessPurchaseCoroutine(
        Customer customer,
        List<Drug> drugs)
    {
        processing = true;

        Debug.Log(
            $"Procesando compra para {customer.name}. " +
            $"Medicamentos: {drugs.Count}"
        );

        // ==========================================
        // 1. APLICAR MEDICAMENTOS
        // ==========================================

        foreach (Drug drug in drugs)
        {
            if (drug == null)
                continue;

            if (drug.Data == null)
            {
                Debug.LogWarning(
                    "CashRegister: Se encontró un Drug sin DrugData."
                );

                continue;
            }

            Debug.Log(
                $"Aplicando medicamento: {drug.Data.DrugName}"
            );

            // De momento usamos el CharacterStats
            // que ya utiliza Drug.Apply().
            CharacterStats characterStats =
                customer.GetComponent<CharacterStats>();

            if (characterStats == null)
            {
                Debug.LogWarning(
                    $"El cliente {customer.name} no tiene CharacterStats."
                );

                continue;
            }

            drug.Apply(characterStats);
        }

        // ==========================================
        // 2. ESPERAR PARA MOSTRAR EL RESULTADO
        // ==========================================

        yield return new WaitForSeconds(delayBeforeEvaluation);

        // ==========================================
        // 3. EVALUAR CLIENTE
        // ==========================================

        TreatmentResult result =
            customer.Evaluate(precioBaseTratamiento);

        Debug.Log(
            $"Cliente evaluado. " +
            $"Satisfacción: {result.Satisfaction}% | " +
            $"Propina: ${result.Tip}"
        );

        // ==========================================
        // 4. ACTUALIZAR ECONOMÍA
        // ==========================================

        if (EconomyUI.Instance != null)
        {
            EconomyUI.Instance.AddEarnings(
                precioBaseTratamiento,
                result.Tip,
                result.Satisfaction
            );
        }

        // ==========================================
        // 5. ESPERAR ANTES DE AVANZAR
        // ==========================================

        yield return new WaitForSeconds(delayBeforeNextCustomer);

        // ==========================================
        // 6. ELIMINAR MEDICAMENTOS DEL ÁREA
        // ==========================================

        ClearTreatmentSlots(drugs);

        // ==========================================
        // 7. FINALIZAR CLIENTE
        // ==========================================

        customerQueue.CustomerFinished();

        if (!customerQueue.HasCustomers())
        {
            if (EconomyUI.Instance != null)
            {
                EconomyUI.Instance.MostrarAvisoSinClientes();
            }
        }

        processing = false;
    }

    private List<Drug> GetDrugsFromTreatmentSlots()
    {
        List<Drug> drugs = new();

        if (treatmentSlots == null)
            return drugs;

        Drug drug = treatmentSlots[0].RemoveDrug();
        while (drug != null)
        {
            drugs.Add(drug);
            drug = treatmentSlots[0].RemoveDrug();
        }

        return drugs;
    }

    private void ClearTreatmentSlots(List<Drug> drugs)
    {
        if (treatmentSlots == null)
            return;

        foreach (Drug drug in drugs)
        {
            if (drug == null)
                continue;

            Destroy(drug.gameObject);
            
        }
    }
}