using System.Collections.Generic;
using UnityEngine;

public class DrugSlot : MonoBehaviour
{
    [Header("Slot")]
    [SerializeField, Min(1)] private int capacity = 1;

    [Header("Permissions")]
    [SerializeField] private bool canTake = true;
    [SerializeField] private bool canPlace = true;

    [Header("Position")]
    [SerializeField] private Transform slotTransform;

    [Header("Stack")]
    [SerializeField] private float stackOffset = 0.05f;

    [SerializeField]private List<Drug> drugs = new();

    public int Capacity => capacity;

    public int Count => drugs.Count;

    public bool IsEmpty => drugs.Count == 0;

    public bool IsFull => drugs.Count >= capacity;

    public bool CanTake => canTake;

    public bool CanPlace => canPlace;

    public Drug CurrentDrug
    {
        get
        {
            if (IsEmpty)
                return null;

            return drugs[drugs.Count - 1];
        }
    }

    private void Awake()
    {
        if (slotTransform == null)
            slotTransform = transform;
    }

    public bool PlaceDrug(Drug drug)
    {
        if (drug == null)
            return false;

        if (!canPlace)
            return false;

        if (IsFull)
            return false;

        drugs.Add(drug);

        drug.transform.SetParent(slotTransform);

        drug.transform.localPosition = new Vector3(-stackOffset , stackOffset , -stackOffset )* (drugs.Count - 1);

        drug.transform.localRotation = Quaternion.identity;

        return true;
    }

    public Drug RemoveDrug()
    {
        if (!canTake)
            return null;

        if (IsEmpty)
            return null;

        int lastIndex = drugs.Count - 1;

        Drug drug = drugs[lastIndex];

        drugs.RemoveAt(lastIndex);

        drug.transform.SetParent(null);

        return drug;
    }
}