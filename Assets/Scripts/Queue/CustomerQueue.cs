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
        foreach (Customer customer in startingCustomers)
        {
            if (customer != null)
                AddCustomer(customer);
        }
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

        UpdateQueuePositions();
    }

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