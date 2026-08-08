using UnityEngine;
using UnityEngine.EventSystems; // Requerido para detectar el clic directamente

public class PillBottle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private DrugData drugData;
    [SerializeField] private CustomerQueue customerQueue;

    // Este método se dispara SIEMPRE que haces clic sobre el objeto con Collider
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("¡Clic detectado con IPointerClickHandler!");

        if (customerQueue == null || !customerQueue.HasCustomers())
            return;

        Customer currentCustomer = customerQueue.CurrentCustomer;

        if (currentCustomer != null && drugData != null)
        {
            CharacterStats stats = currentCustomer.GetComponent<CharacterStats>();
            if (stats != null)
            {
                foreach (var modifier in drugData.StatModifiers)
                {
                    stats.ApplyModifier(modifier);
                }
                Debug.Log($"Se aplico {drugData.DrugName} al cliente.");
            }
        }
    }
}
