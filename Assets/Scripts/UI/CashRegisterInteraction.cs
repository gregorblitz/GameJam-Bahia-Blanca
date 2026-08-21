using UnityEngine;
using UnityEngine.EventSystems;

public class CashRegisterInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private CashRegister cashRegister;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private static readonly int OutlineEnabled = Shader.PropertyToID("_OutlineEnabled");
    private Material material;
    public void Start()
    {

        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Optional: Add visual feedback for hover state

        
        Debug.Log("Mouse entered CashRegisterInteraction");
        SetHighlight(true);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        // Optional: Remove visual feedback for hover state
        Debug.Log("Mouse exited CashRegisterInteraction");
        SetHighlight(false);
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
        private void SetHighlight(bool state)
    {
        
        if (material != null)
        {
            material.SetFloat(OutlineEnabled, state ? 1f : 0f);
        }else
        {
            Debug.LogWarning("Material is not assigned in CashRegisterInteraction.");
        }
    }
}