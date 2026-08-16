using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Drug heldDrug;

    [SerializeField] private Transform handSlot;

    public Drug HeldDrug => heldDrug;

    public bool IsEmpty => heldDrug == null;

    public bool PickUp(Drug drug)
    {
        if (drug == null || !IsEmpty)
            return false;

        heldDrug = drug;

        drug.transform.SetParent(handSlot);
        drug.transform.localPosition = Vector3.zero;
        drug.transform.localRotation = Quaternion.identity;

        return true;
    }

    public Drug Drop()
    {
        if (heldDrug == null)
            return null;

        Drug drug = heldDrug;

        heldDrug = null;

        drug.transform.SetParent(null);

        return drug;
    }
}