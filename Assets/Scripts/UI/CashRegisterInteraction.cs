using UnityEngine;
using UnityEngine.EventSystems;

public class CashRegisterInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer highlightObject;
    [SerializeField] private CashRegister cashRegister;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Optional: Add visual feedback for hover state
        if(highlightObject != null)
        highlightObject.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Optional: Remove visual feedback for hover state
        if(highlightObject != null)
        highlightObject.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Optional: Add visual feedback for click state
        Debug.Log("Mouse clicked CashRegisterInteraction");
        if (cashRegister != null)
        {
            cashRegister.ProcessPurchase();
        }
        else
        {
            Debug.LogWarning("CashRegister reference is not assigned in CashRegisterInteraction.");
        }
    }
}