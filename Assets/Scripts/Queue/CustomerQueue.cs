using System.Collections.Generic;
using UnityEngine;

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

            if (customer == null)
                continue;

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