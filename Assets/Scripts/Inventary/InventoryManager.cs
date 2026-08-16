using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [System.Serializable]
    public class InventoryEntry
    {
        public DrugData drugData;
        [Min(0)]
        public int quantity;
    }

    [SerializeField] private List<InventoryEntry> inventory = new();

    private Dictionary<DrugData, int> quantities = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        BuildDictionary();
    }

    private void BuildDictionary()
    {
        quantities.Clear();

        foreach (InventoryEntry entry in inventory)
        {
            if (entry.drugData == null)
                continue;

            quantities[entry.drugData] = entry.quantity;
        }
    }

    public int GetQuantity(DrugData drugData)
    {
        if (drugData == null)
            return 0;

        if (quantities.TryGetValue(drugData, out int quantity))
            return quantity;

        return 0;
    }

    public bool HasDrug(DrugData drugData)
    {
        return GetQuantity(drugData) > 0;
    }

    public bool RemoveDrug(DrugData drugData, int amount = 1)
    {
        if (drugData == null || amount <= 0)
            return false;

        int currentQuantity = GetQuantity(drugData);

        if (currentQuantity < amount)
            return false;

        quantities[drugData] = currentQuantity - amount;

        return true;
    }

    public void AddDrug(DrugData drugData, int amount = 1)
    {
        if (drugData == null || amount <= 0)
            return;

        if (quantities.ContainsKey(drugData))
            quantities[drugData] += amount;
        else
            quantities.Add(drugData, amount);
    }
}