using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PillBottle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private DrugData drugData;
    [SerializeField] private CustomerQueue customerQueue;
    [SerializeField] private float precioBaseTratamiento = 50f;

    private static bool procesandoTransicion = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Si estamos esperando a que pase el siguiente cliente, ignoramos clics
        if (procesandoTransicion)
            return;

        if (customerQueue == null || !customerQueue.HasCustomers())
        {
            Debug.LogWarning("No hay clientes en la fila.");
            return;
        }

        Customer currentCustomer = customerQueue.CurrentCustomer;

        if (currentCustomer != null && drugData != null)
        {
            // Creamos o recuperamos el componente Drug
            Drug drugComponent = gameObject.GetComponent<Drug>();
            if (drugComponent == null)
            {
                drugComponent = gameObject.AddComponent<Drug>();
            }

            // Asignamos la información de la droga mediante reflexión al campo privado
            var field = typeof(Drug).GetField("data", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(drugComponent, drugData);
            }

            // Intentamos entregar la medicina
            bool exito = currentCustomer.ReceiveDrug(drugComponent);

            if (exito)
            {
                Debug.Log($"¡Dosis de {drugData.DrugName} entregada!");

                // Verificamos si alcanzamos la cantidad máxima permitida para este cliente
                // Accedemos a la lista de drogas recibidas
                var receivedDrugsField = typeof(Customer).GetField("receivedDrugs", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var customerDataField = typeof(Customer).GetField("data", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (receivedDrugsField != null && customerDataField != null)
                {
                    var receivedDrugs = receivedDrugsField.GetValue(currentCustomer) as System.Collections.IList;
                    var customerData = customerDataField.GetValue(currentCustomer) as CustomerData;

                    if (receivedDrugs != null && customerData != null)
                    {
                        if (receivedDrugs.Count >= customerData.MaxDrugs)
                        {
                            Debug.Log("Límite de medicinas alcanzado. Evaluando y avanzando fila automáticamente...");
                            StartCoroutine(AvanzarSiguienteClienteCo(currentCustomer));
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("El cliente no acepta más medicinas.");
            }
        }
    }

    private IEnumerator AvanzarSiguienteClienteCo(Customer cliente)
    {
        procesandoTransicion = true;

        // Pausa para que el jugador observe los stats finales
        yield return new WaitForSeconds(1.5f);

        // Evalua el cliente
        TreatmentResult resultado = cliente.Evaluate(precioBaseTratamiento);
        Debug.Log($"Satisfacción: {resultado.Satisfaction}% | Propina: ${resultado.Tip}");

        // Actualiza la UI pasando base, propina y satisfacción
        if (EconomyUI.Instance != null)
        {
            EconomyUI.Instance.AddEarnings(precioBaseTratamiento, resultado.Tip, resultado.Satisfaction);
        }

        // Espera 2 segundos para que el jugador lea el mensaje de satisfacción
        yield return new WaitForSeconds(2.0f);

        // Hace avanzar la fila
        customerQueue.CustomerFinished();

        // Verifica si la fila quedó vacía tras terminar
        if (!customerQueue.HasCustomers())
        {
            if (EconomyUI.Instance != null)
            {
                EconomyUI.Instance.MostrarAvisoSinClientes();
            }
        }

        yield return new WaitForSeconds(0.3f);
        procesandoTransicion = false;
    }
}