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


public class CustomerQueue : MonoBehaviour
{
    [Header("Customers for testing")]
    [SerializeField] private List<Customer> startingCustomers = new();

    [Header("Queue Positions")]
    [SerializeField] private Transform[] customerSpawnPoints;

    //**********SCM-INI-COLA-CONTINUA *************
    [Header("Continuous Queue System")]
    [Tooltip("Arrastra aquí los prefabs de clientes que irán saliendo")]
    [SerializeField] private List<GameObject> availableCustomerPrefabs = new();
    [SerializeField] private int maxVisibleCustomers = 3; // Mantiene 3 visibles siempre
    [SerializeField] private bool infiniteQueue = true; // Si es false, se detiene tras el total del día
    [SerializeField] private int totalCustomersForDay = 10;

    private int totalSpawnedCount = 0;
    //**********SCM-FIN-COLA-CONTINUA *************

    private readonly List<Customer> customers = new();

    public Customer CurrentCustomer
    {
        get
        {
            if (customers.Count == 0)
                return null;

            return customers[0];
        }
    }

    private void Start()
    {
        // Añade clientes iniciales si existen en la lista de prueba
        foreach (Customer customer in startingCustomers)
        {
            if (customer != null)
                AddCustomer(customer);
        }

        //**********SCM-INI-COLA-CONTINUA *************
        // Llena la fila inicial hasta completar el cupo visible (3 clientes)
        RefillQueue();
        //**********SCM-FIN-COLA-CONTINUA *************
    }

    public void AddCustomer(Customer customer)
    {
        if (customer == null)
            return;

        customers.Add(customer);

        UpdateQueuePositions();
    }

    public void CustomerFinished()
    {
        if (customers.Count == 0)
            return;

        Customer finishedCustomer = customers[0];

        customers.RemoveAt(0);

        //**********SCM-INI-BUG1 *************
        // Destruye la representación visual instanciada 
        if (finishedCustomer != null)
        {
            if (finishedCustomer.CustomerModel != null)
            {
                Destroy(finishedCustomer.CustomerModel);
            }
            Destroy(finishedCustomer.gameObject);
        }
        //**********SCM-FIN-BUG1 *************
        // finishedCustomer.LeaveQueue();

        //**********SCM-INI-COLA-CONTINUA *************
        // Al salir un cliente, genera inmediatamente el siguiente en la última posición
        if (CanSpawnMore())
        {
            SpawnNextCustomer();
        }
        //**********SCM-FIN-COLA-CONTINUA *************

        UpdateQueuePositions();
    }

    //**********SCM-INI-COLA-CONTINUA *************
    private void RefillQueue()
    {
        while (customers.Count < maxVisibleCustomers && CanSpawnMore())
        {
            SpawnNextCustomer();
        }
        UpdateQueuePositions();
    }

    private bool CanSpawnMore()
    {
        if (availableCustomerPrefabs == null || availableCustomerPrefabs.Count == 0)
            return false;

        if (infiniteQueue)
            return true;

        return totalSpawnedCount < totalCustomersForDay;
    }

    private void SpawnNextCustomer()
    {
        if (availableCustomerPrefabs.Count == 0)
            return;

        // Filtramos para obtener solo prefabs que NO estén actualmente en la fila
        List<GameObject> candidates = new();

        foreach (GameObject prefab in availableCustomerPrefabs)
        {
            if (prefab == null) continue;

            Customer prefabCustomer = prefab.GetComponent<Customer>();
            bool alreadyInQueue = false;

            if (prefabCustomer != null && prefabCustomer.Data != null)
            {
                foreach (Customer activeCustomer in customers)
                {
                    if (activeCustomer != null && activeCustomer.Data == prefabCustomer.Data)
                    {
                        alreadyInQueue = true;
                        break;
                    }
                }
            }

            if (!alreadyInQueue)
            {
                candidates.Add(prefab);
            }
        }

        // Si todos los prefabs estuvieran ocupados por alguna razón, usamos la lista completa como respaldo
        List<GameObject> selectionList = candidates.Count > 0 ? candidates : availableCustomerPrefabs;

        int randomIndex = Random.Range(0, selectionList.Count);
        GameObject selectedPrefab = selectionList[randomIndex];

        if (selectedPrefab != null)
        {
            GameObject newObj = Instantiate(selectedPrefab);
            Customer newCustomer = newObj.GetComponent<Customer>();

            if (newCustomer != null)
            {
                customers.Add(newCustomer);
                totalSpawnedCount++;
            }
        }
    }
    //**********SCM-FIN-COLA-CONTINUA *************

    private void UpdateQueuePositions()
    {
        int positionCount = Mathf.Min(customers.Count, customerSpawnPoints.Length);

        for (int i = 0; i < positionCount; i++)
        {
            if (customerSpawnPoints[i] == null)
                continue;

            Customer customer = customers[i];
            //**********SCM-INI-BUG1 *************
            //if (customer == null)
            //    continue;
            if (customer == null || customer.CustomerModel == null)
                continue;
            //**********SCM-FIN-BUG1 *************
            Transform customerTransform = customer.CustomerModel.transform;

            // Lo hacemos hijo del punto de la fila.
            customerTransform.SetParent(customerSpawnPoints[i]);

            // Eliminamos cualquier posición/rotación local anterior.
            customerTransform.localPosition = Vector3.zero;
            customerTransform.localRotation = Quaternion.identity;
            customerTransform.localScale = Vector3.one;
        }
    }

    public bool HasCustomers()
    {
        return customers.Count > 0;
    }

    public int GetCustomerCount()
    {
        return customers.Count;
    }

    public Customer GetCustomerAt(int index)
    {
        if (index < 0 || index >= customers.Count)
            return null;

        return customers[index];
    }
}