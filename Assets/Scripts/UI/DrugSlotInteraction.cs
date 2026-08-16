using UnityEngine;

using UnityEngine.EventSystems;
public class DrugSlotInteraction : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private DrugSlot slot;
    [SerializeField] private PlayerHand playerHand;

    [Header("Highlight")]
    [SerializeField] private GameObject highlightObject;

    private void Awake()
    {
        if (slot == null)
            slot = GetComponent<DrugSlot>();

        SetHighlight(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered DrugSlotInteraction");
        SetHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetHighlight(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        HandleClick();
    }

    private void HandleClick()
    {
        
        Debug.Log("Mouse clicked on DrugSlotInteraction");
        if (slot == null || playerHand == null)
            return;

        // CASO 1:
        // El slot tiene un medicamento y la mano está vacía.
        if (!slot.IsEmpty && playerHand.IsEmpty)
        {
            Drug drug = slot.RemoveDrug();

            if (drug != null)
            {
                playerHand.PickUp(drug);
            }

            return;
        }

        // CASO 2:
        // El slot está vacío y tenemos un medicamento en la mano.
        if (!slot.IsFull && !playerHand.IsEmpty)
        {
            Drug drug = playerHand.Drop();

            if (drug != null)
            {
                bool placed = slot.PlaceDrug(drug);

                // Si por alguna razón no pudo colocarse,
                // devolvemos el medicamento a la mano.
                if (!placed)
                {
                    playerHand.PickUp(drug);
                }
            }

            return;
        }

        // CASO 3:
        // Slot ocupado y tenemos otro medicamento en la mano.
        if (slot.IsFull && !playerHand.IsEmpty)
        {
            Drug slotDrug = slot.RemoveDrug();
            Drug handDrug = playerHand.Drop();

            if (slotDrug != null && handDrug != null)
            {
                playerHand.PickUp(slotDrug);
                slot.PlaceDrug(handDrug);
            }
        }
    }

    private void SetHighlight(bool state)
    {
        if (highlightObject != null)
            highlightObject.SetActive(state);
    }
}