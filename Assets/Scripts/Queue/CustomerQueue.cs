using System.Collections.Generic;
using UnityEngine;

public class CustomerQueue : MonoBehaviour
{
    private readonly Queue<Customer> queue = new();

    public Customer CurrentCustomer { get; private set; }

    public void AddCustomer(Customer customer)
    {
        queue.Enqueue(customer);

        if (CurrentCustomer == null)
            SpawnNextCustomer();
    }

    public void CustomerFinished()
    {
        if (CurrentCustomer == null)
            return;

        CurrentCustomer = null;

        SpawnNextCustomer();
    }

    private void SpawnNextCustomer()
    {
        if (queue.Count == 0)
            return;

        CurrentCustomer = queue.Dequeue();

        // Aquí luego puedes moverlo al mostrador,
        // reproducir una animación, etc.
    }

    public bool HasCustomers()
    {
        return CurrentCustomer != null || queue.Count > 0;
    }
}